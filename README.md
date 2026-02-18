# medicalgastroproject

Production-oriented blueprint and starter skeleton for a medical clinic platform supporting:
- Appointment scheduling and real-time queue.
- Generic EMR-style studies with image/document/video attachments.
- Offline-first local persistence and outbox sync.
- AI-assisted attachment analysis and report drafting.
- Deterministic Word report rendering with Open XML.

## Project layout
- `docs/medical-clinic-blueprint.md`: architecture, schemas, API, sync model, security and milestones.
- `docs/how-to-start.md`: local development startup guide.
- `scripts/local-dev.sh`: build-and-run helper for local backend/frontend startup (macOS/Linux).
- `scripts/local-dev.ps1`: build-and-run helper for local backend/frontend startup (Windows PowerShell).
- `backend/src/Clinic.Api`: ASP.NET Core .NET 8 API skeleton.
- `workstation/Clinic.Workstation`: local SQLite + encrypted local storage + outbox worker skeleton.
- `frontend`: React TypeScript SPA skeleton.

## Quick start
See `docs/how-to-start.md` for complete instructions.
