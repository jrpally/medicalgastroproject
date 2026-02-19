# medicalgastroproject

Production-oriented blueprint and starter skeleton for a medical clinic platform with:
- Appointment scheduling and real-time queue management.
- Generic EMR-style studies with attachments (images/documents/videos).
- Offline-first behavior (local store + outbox sync).
- AI-assisted attachment analysis and report drafting.
- Deterministic Word report generation with Open XML.

## Main documentation (direct links)
- [Startup and local run guide](./docs/how-to-start.md)
- [Complete technical blueprint (architecture, data models, API, security)](./docs/medical-clinic-blueprint.md)

## Repository structure
- [backend/src/Clinic.Api](./backend/src/Clinic.Api): ASP.NET Core .NET 8 API (REST + SignalR).
- [frontend](./frontend): React + TypeScript SPA.
- [workstation/Clinic.Workstation](./workstation/Clinic.Workstation): offline modules (SQLite, encryption, outbox worker) in skeleton form.
- [scripts/local-dev.sh](./scripts/local-dev.sh): starts backend + frontend on macOS/Linux.
- [scripts/local-dev.ps1](./scripts/local-dev.ps1): starts backend + frontend on Windows PowerShell.

## Quick configuration

### 1) Prerequisites
- .NET SDK 8+
- Node.js 20+ and npm
- (Optional) Azurite or real Azure resources

### 2) Relevant configuration notes
> Current status: scaffold. Several integrations are placeholders and need environment-specific wiring.

- **Backend auth**: JWT Bearer enabled with role policies (`Administrator`, `Secretary`, `Doctor`).
- **Calendar integration**: `GoogleCalendarSyncService` currently exists as a scaffold (not production OAuth integration yet).
- **Storage/queue**: Azure abstractions (Tables/Blob/Queue) are present and ready for real implementation wiring.

### 3) Local startup (recommended)
- **Windows PowerShell**:
  ```powershell
  ./scripts/local-dev.ps1
  ```
- **macOS/Linux**:
  ```bash
  ./scripts/local-dev.sh
  ```

For full step-by-step instructions, see:
- [docs/how-to-start.md](./docs/how-to-start.md)

## Frontend demo routes (scaffold)
- `/admin/users`
- `/secretary/calendar`
- `/doctor/worklist`
- `/doctor/patients/:id/timeline`
- `/doctor/studies/:studyId`
- `/doctor/reports/:draftId`

## Project status
This repository is currently in **starter/skeleton** mode: it is intended to validate architecture, contracts, and baseline workflow structure, but still requires full production integration (infrastructure, real Google OAuth integration, complete persistence, and end-to-end testing).
