# Medical Clinic Platform Blueprint (Production-Grade)

## Assumptions
- Clinic workstations run Windows 10/11 and have intermittent connectivity.
- A cloud API is available over HTTPS and secured with Entra ID for staff.
- Patient portal authentication uses OTP (SMS/email) with short-lived tokens.
- Azure Table Storage is selected for cost-effective key-value query patterns; if global distribution/multi-region low-latency becomes mandatory, migrate to Cosmos DB Table API with minimal code changes.
- All timestamps are stored as UTC ISO-8601.

---

## 1) System Overview Diagram (Text) + Module Responsibilities

```text
┌──────────────────────────────────────────────────────────────────────────────┐
│                           Clinic Workstation (Windows)                      │
│  React SPA + Electron/Host Shell                                            │
│  - Calendar/Queue/Timeline/Study/Report UIs                                 │
│  - Offline Banner + Conflict UI                                              │
│  - Local SQLite + Encrypted File Store + Outbox Worker                      │
└───────────────┬──────────────────────────────────────────────────────────────┘
                │ HTTPS/SignalR
                ▼
┌──────────────────────────────────────────────────────────────────────────────┐
│                        ASP.NET Core API (.NET 8)                            │
│ Controllers | Validation | Idempotency | RBAC | Audit | Telemetry           │
│ Services: Scheduling, Timeline, Study, Attachment, AI, Reports, Sync        │
│ Repositories: Table Storage + Blob Storage + Queue Storage adapters          │
│ SignalR Hub: queue updates, AI/report completion, timeline events            │
└───────────────┬───────────────┬──────────────────────────────┬───────────────┘
                │               │                              │
                ▼               ▼                              ▼
      Azure Table Storage  Azure Blob Storage         Azure Queue Storage
      (core entities)      (attachments/docx)         (AI/report async jobs)
                │                                            │
                └────────────────────────────────────────────┘
                                     ▼
                               AI Worker Service
                    (vision + document extraction + prompt safety)
```

### Module responsibilities
- **Frontend SPA**: local-first UX, optimistic writes, outbox status, conflict resolution.
- **Workstation Sync Engine**: durable local queue, retry/backoff, idempotent replay, chunked upload resume.
- **API**: business rules, anti-double-booking, role enforcement, audit, deterministic report rendering orchestration.
- **AI Worker**: isolated prompt execution; emits structured JSON only, confidence, disclaimers.
- **Storage layer**: deterministic keys for timeline/read patterns and immutable analysis/report records.

---

## 2) Data Model (Azure Table + SQLite) with Keys and Examples

## 2.1 Azure Table design

> Convention: `PartitionKey` optimized for primary query; `RowKey` sortable with reverse ticks where timeline order is needed.

### Patient
- Table: `Patients`
- PK: `PATIENT#{PatientId}`
- RK: `PROFILE`
- Fields: demographics, contact, consent flags, created/updated metadata.

### Provider
- Table: `Providers`
- PK: `PROVIDER#{ProviderId}`
- RK: `PROFILE`

### Appointment
- Table: `Appointments`
- PK: `PROVIDER#{ProviderId}#DAY#{yyyyMMdd}`
- RK: `{StartTimeHHmm}#{AppointmentId}`
- Secondary index table (`AppointmentsByPatient`):
  - PK: `PATIENT#{PatientId}`
  - RK: `{yyyyMMddHHmm}#{AppointmentId}`

### AvailabilityTemplate / Exception
- `AvailabilityTemplates`: PK `PROVIDER#{ProviderId}` RK `DOW#{0-6}#TEMPLATE#{TemplateId}`
- `AvailabilityExceptions`: PK `PROVIDER#{ProviderId}#DAY#{yyyyMMdd}` RK `EX#{ExceptionId}`

### Encounter
- Table: `Encounters`
- PK: `PATIENT#{PatientId}`
- RK: `ENC#{ReverseTicks}#{EncounterId}`

### Study (generic)
- Table: `Studies`
- PK: `PATIENT#{PatientId}`
- RK: `STD#{ReverseTicks}#{StudyId}`
- Columns: `StudyType`, `Status`, `OrderedBy`, `PerformedBy`, `DateTimePerformed`, `NarrativeJson`, `StructuredFindingsJson`, `EncounterId?`.

### Attachment
- Table: `Attachments`
- PK: `STUDY#{StudyId}`
- RK: `ATT#{AttachmentId}`
- Contains blob reference, checksum, content type, byte size, encryption metadata.

### AttachmentIndex (patient/date/type)
- Table: `AttachmentIndex`
- PK: `PATIENT#{PatientId}#TYPE#{AttachmentType}`
- RK: `{yyyyMMddHHmm}#{AttachmentId}`

### AIAnalysisJob
- Table: `AIJobs`
- PK: `ATTACHMENT#{AttachmentId}`
- RK: `JOB#{JobId}`
- Fields: model version, status, requestedBy, submittedAt, completedAt.

### AIAnalysisResult (immutable)
- Table: `AIResults`
- PK: `JOB#{JobId}`
- RK: `RESULT#{TimestampTicks}`
- Immutable JSON payload.

### ReportDraft
- Table: `ReportDrafts`
- PK: `STUDY#{StudyId}`
- RK: `DRAFT#{DraftId}`

### ReportFinal
- Table: `ReportFinals`
- PK: `STUDY#{StudyId}`
- RK: `FINAL#{ReportId}`
- Blob URI + hash + signer/finalizer metadata.

### AuditLog
- Table: `AuditLogs`
- PK: `TENANT#{TenantId}#DAY#{yyyyMMdd}`
- RK: `{TimestampTicks}#{AuditId}`

### OutboxQueue (server-side optional)
- Table: `ServerOutbox`
- PK: `NODE#{NodeName}`
- RK: `MSG#{CreatedTicks}#{MessageId}`

## 2.2 SQLite mirror schema (workstation)

```sql
CREATE TABLE patients (
  id TEXT PRIMARY KEY,
  json TEXT NOT NULL,
  version TEXT NOT NULL,
  updated_utc TEXT NOT NULL
);

CREATE TABLE appointments (
  id TEXT PRIMARY KEY,
  provider_id TEXT NOT NULL,
  patient_id TEXT NOT NULL,
  start_utc TEXT NOT NULL,
  end_utc TEXT NOT NULL,
  status TEXT NOT NULL,
  local_state TEXT NOT NULL DEFAULT 'synced',
  last_error TEXT NULL,
  row_version TEXT NULL
);

CREATE TABLE studies (
  id TEXT PRIMARY KEY,
  patient_id TEXT NOT NULL,
  encounter_id TEXT NULL,
  study_type TEXT NOT NULL,
  status TEXT NOT NULL,
  datetime_performed_utc TEXT NOT NULL,
  narrative_json TEXT NULL,
  findings_json TEXT NULL,
  local_state TEXT NOT NULL,
  row_version TEXT NULL
);

CREATE TABLE attachments (
  id TEXT PRIMARY KEY,
  study_id TEXT NOT NULL,
  patient_id TEXT NOT NULL,
  kind TEXT NOT NULL,
  content_type TEXT NOT NULL,
  local_path TEXT NOT NULL,
  blob_name TEXT NULL,
  sha256 TEXT NOT NULL,
  size_bytes INTEGER NOT NULL,
  upload_state TEXT NOT NULL,
  local_state TEXT NOT NULL
);

CREATE TABLE outbox (
  id TEXT PRIMARY KEY,
  aggregate_type TEXT NOT NULL,
  aggregate_id TEXT NOT NULL,
  operation TEXT NOT NULL,
  payload_json TEXT NOT NULL,
  idempotency_key TEXT NOT NULL,
  attempt_count INTEGER NOT NULL DEFAULT 0,
  next_attempt_utc TEXT NOT NULL,
  status TEXT NOT NULL,
  last_error TEXT NULL,
  created_utc TEXT NOT NULL
);

CREATE UNIQUE INDEX ux_outbox_idempotency ON outbox(idempotency_key);
```

## 2.3 Example payloads

```json
{
  "studyId": "b58cf647-2e4e-4f55-bce6-8a89b53c5c95",
  "patientId": "P-100221",
  "studyType": "Labs",
  "status": "Draft",
  "dateTimePerformed": "2026-02-17T10:30:00Z",
  "structuredFindings": {
    "panels": [
      {"name": "CBC", "items": [{"name": "Hemoglobin", "value": 10.8, "unit": "g/dL"}]}
    ]
  }
}
```

---

## 3) API Spec with DTOs (REST + SignalR)

## 3.1 Common headers
- `Authorization: Bearer <token>`
- `x-idempotency-key: <GUID>` required for create/update transitions.
- `If-Match: <etag>` for optimistic concurrency on PATCH/finalize.

## 3.2 Error model

```json
{
  "code": "APPOINTMENT_CONFLICT",
  "message": "Selected slot is no longer available",
  "details": [{"field": "startUtc", "issue": "overlaps existing appointment"}],
  "correlationId": "00-..."
}
```

## 3.3 Endpoints

### Slots and Appointments
- `GET /slots?providerId=&from=&to=&studyType=`
  - Returns free/reserved slots with policy flags.
- `POST /appointments`
  - Body: `CreateAppointmentRequest`.
  - Validation: start<end, within availability, no overlap (atomic transaction lock by provider/day partition).
- `POST /appointments/{id}/arrive`
- `POST /appointments/{id}/ready`
- `POST /appointments/{id}/start`
- `POST /appointments/{id}/complete`

### Patients and Timeline
- `GET /patients/search?q=&dob=&phone=`
- `GET /patients/{id}/timeline?from=&to=` includes appointments + encounters + studies + attachment refs.

### Studies
- `POST /studies`
- `PATCH /studies/{id}`
- `GET /studies/{id}`
- `GET /studies?patientId=&type=&from=&to=`

### Attachments
- `POST /attachments/upload/init`
- `PUT /attachments/upload/chunk`
- `POST /attachments/upload/complete`
- `POST /studies/{id}/attachments/{attachmentId}/link`

### AI
- `POST /attachments/{id}/analyze`
- `GET /attachments/{id}/analysis`
- `POST /studies/{id}/ai/draft-report-json`

### Reports
- `POST /reports/render-docx`
- `POST /reports/{id}/finalize`

## 3.4 DTO highlights
- `CreateStudyRequest`: patientId, encounterId?, studyType, orderedBy?, performedBy?, narrative?, structuredFindings?.
- `AnalyzeAttachmentRequest`: attachmentId, taskType (`describe|summarize|extract_labs|draft_interpretation`), locale.
- `RenderReportRequest`: templateId, draftJson, attachmentIds[], citationMode.

---

## 4) Sync/Offline Design with Outbox and Conflict Handling

### Local-first flow
1. UI writes entity to SQLite (`local_state=pending_sync`).
2. Outbox record created in same transaction.
3. Background worker sends queued items by FIFO/priority.
4. On success, updates row_version + `local_state=synced`.

### Idempotency
- Client generates GUID per command.
- API persists processed idempotency keys for 24h+ with response hash.
- Retries return identical response for same key.

### Conflict handling
- Appointments: server returns `409 APPOINTMENT_CONFLICT` with alternatives.
- Study patch conflict: `412 PRECONDITION_FAILED` with current etag.
- Client conflict table drives UI merge/accept-server/reapply-local.

### Security offline
- SQLite encryption key stored via DPAPI-protected secret.
- Local files encrypted AES-GCM per-file DEK; DEK encrypted with DPAPI KEK.
- Offline audit records buffered and synced later.

---

## 5) Attachment Upload and Storage Approach

- Chunk size: 4–8 MB default.
- `upload/init`: creates server upload session + blob staging name + SAS for block upload.
- `upload/chunk`: sends blockId + checksum; server validates SHA-256 chain.
- `upload/complete`: commits block list, verifies final hash/size, writes `Attachment` entity.
- Large uploads resume using persisted session state in SQLite.
- Blob containers:
  - `attachments-private`
  - `reports-final`
- Access only via short-lived user-delegation SAS (read-only, ≤5 min).

---

## 6) AI Job Design + JSON Schemas + Safe Prompt Templates

### Job design
- API enqueues `AIAnalysisRequested` message with attachment metadata.
- Worker fetches blob using managed identity.
- Worker runs policy prompt + schema-constrained response.
- Result persisted immutably in `AIResults`; event pushed via SignalR.

### JSON schema (analysis result)

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://clinic.example/schemas/ai-analysis-result.json",
  "type": "object",
  "required": ["attachmentId", "task", "summary", "extractions", "confidence", "disclaimers"],
  "properties": {
    "attachmentId": {"type": "string", "format": "uuid"},
    "task": {"type": "string"},
    "summary": {"type": "string"},
    "extractions": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["name", "value", "confidence"],
        "properties": {
          "name": {"type": "string"},
          "value": {"type": ["string", "number", "null"]},
          "unit": {"type": ["string", "null"]},
          "referenceRange": {"type": ["string", "null"]},
          "abnormalFlag": {"type": ["string", "null"], "enum": ["low", "high", "critical", null]},
          "confidence": {"type": "number", "minimum": 0, "maximum": 1}
        }
      }
    },
    "confidence": {"type": "number", "minimum": 0, "maximum": 1},
    "disclaimers": {
      "type": "array",
      "items": {"type": "string"},
      "minItems": 1
    },
    "suggestedFollowUps": {
      "type": "array",
      "items": {"type": "string"}
    }
  },
  "additionalProperties": false
}
```

### Safe prompt templates (system excerpt)
- "You are a clinical documentation assistant. Do not diagnose. Provide decision-support only. If uncertain, say uncertain. Output strictly valid JSON per schema."
- "Use provided reference ranges only; do not invent ranges."
- "Always include disclaimer: 'AI output is assistive and must be reviewed by a licensed clinician.'"

---

## 7) Word Report Templating + Rendering (Open XML)

### Pipeline
1. Generate `ReportDraft` JSON from study/encounter + AI summary.
2. Clinician edits sections (history/findings/impression/plan).
3. `render-docx` merges JSON with template placeholders and image citations.
4. Save `.docx` in Blob; create immutable `ReportFinal` entry.
5. Finalization locks draft from further edits.

### Determinism controls
- Fixed template version IDs.
- Deterministic sort of sections and citation order.
- Explicit locale/timezone formatting config.

### Citation format
- Each embedded figure references `AttachmentId` and capture datetime.

---

## 8) Frontend Screens + Component Breakdown + Routing

### Routes
- `/login`
- `/secretary/calendar`
- `/secretary/patients/:id/new-study`
- `/doctor/worklist`
- `/doctor/patients/:id/timeline`
- `/doctor/studies/:studyId`
- `/doctor/reports/:draftId`
- `/portal/book` (optional)

### Shared components
- `OfflineBanner`
- `SyncStatusBadge`
- `ConflictResolutionDrawer`
- `AttachmentUploader` (chunk-aware)
- `AiInsightPanel`
- `TimelineFeed`

### State slices
- auth, connectivity, outbox, appointments, patients, studies, attachments, aiJobs, reports.

---

## 9) Security, Privacy, Audit, Threat Model

### RBAC matrix (minimum)
- Secretary: appointments + demographics + attach docs; no final report signing.
- Doctor: full chart, AI analyze, report finalize.
- Patient: own appointments + optional uploads.

### Audit events
- `ATTACHMENT_VIEWED`, `ATTACHMENT_DOWNLOADED`, `AI_ANALYSIS_REQUESTED`, `AI_ANALYSIS_VIEWED`, `REPORT_DRAFT_RENDERED`, `REPORT_FINALIZED`.

### Threats + mitigations
- Token theft → short-lived access tokens + conditional access + device compliance.
- Blob URL leakage → user delegation SAS + minimal expiry + IP scoping if possible.
- Prompt injection in documents → content sanitization and schema-locked output.
- Offline device theft → full-disk encryption + DPAPI-encrypted keys + remote wipe policy.

### Retention/export
- Policy-based retention by entity type.
- Export bundle includes audit trail and signed hash manifest.

---

## 10) Implementation Plan + Milestones + Folder Structure

### Milestones
1. **Foundation**: auth, storage adapters, base entities, telemetry.
2. **Scheduling**: slots, booking engine, anti-double-booking + SignalR.
3. **Studies/Attachments**: study CRUD + chunked upload + timeline.
4. **Offline Sync**: SQLite mirror + outbox worker + conflicts UI.
5. **AI**: analysis jobs + structured results + safe prompts.
6. **Reports**: draft editor + OpenXML rendering + finalization.
7. **Hardening**: audit completeness, penetration checks, performance/load test.

### Folder structure

```text
/backend/src/Clinic.Api
/backend/src/Clinic.Workers.AI
/workstation/Clinic.Workstation
/frontend
/docs
```

---

## 11) Minimal Code Skeletons (map)
- ASP.NET Core controllers/services/repositories in `backend/src/Clinic.Api`.
- Azure Storage access layer in `backend/src/Clinic.Api/Storage`.
- SQLite store and outbox worker in `workstation/Clinic.Workstation`.
- React pages/components/API client in `frontend/src`.
- SignalR hub in `backend/src/Clinic.Api/Hubs`.

---

## 12) Explicit LLM Rule
**Return all JSON schemas using JSON Schema Draft 2020-12.**
