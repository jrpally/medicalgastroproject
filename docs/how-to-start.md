# How to Start the Application (Development)

This repository currently contains production-oriented **skeletons** for backend, workstation sync components, and frontend SPA.

## Prerequisites
- Windows 10/11 (recommended for workstation modules)
- .NET SDK 8.0+
- Node.js 20+ and npm
- (Optional) Azure Storage Emulator/Azurite or Azure resources for real integration

## One-command local startup

A convenience script is provided to build projects and run backend + frontend together:

- macOS/Linux (bash):

```bash
./scripts/local-dev.sh
```

- Windows (PowerShell):

```powershell
./scripts/local-dev.ps1
```

What this script does:
1. Restores and builds backend API.
2. Restores and builds workstation module.
3. Installs frontend dependencies.
4. Starts backend on `https://localhost:5001` (also `http://localhost:5000`).
5. Starts frontend Vite server on `http://localhost:5173`.

Stop all services with `Ctrl+C`.

## Manual startup (if preferred)

## 1) Start Backend API (`Clinic.Api`)

```bash
cd backend/src/Clinic.Api
dotnet restore
dotnet run
```

Expected default behavior:
- API starts with Swagger enabled.
- SignalR hub endpoint is available at `/hubs/events`.

## 2) Start Frontend SPA

```bash
cd frontend
npm install
npm run dev
```

Expected default behavior:
- Vite dev server starts (typically at `http://localhost:5173`).
- Starter workflow routes are available:
  - `/admin/users` (admin staff management: doctors/secretaries)
  - `/secretary/calendar`
  - `/doctor/worklist`
  - `/doctor/patients/:id/timeline`
  - `/doctor/studies/:studyId`
  - `/doctor/reports/:draftId`

## 3) Workstation Module (Offline/Sync skeleton)

The workstation project is a library-style skeleton. It does not yet have a host executable.

To compile:

```bash
cd workstation/Clinic.Workstation
dotnet restore
dotnet build
```

## 4) Suggested local run order
1. Start backend API.
2. Start frontend dev server.
3. Open the frontend URL in browser.

## 5) Current implementation status
- Core architecture and API contracts are scaffolded.
- Storage and queue integrations contain TODO markers for environment-specific wiring.
- Offline sync worker and encrypted local store are skeleton implementations.

## Secretary appointment workflow notes
- The secretary page now sends `POST /appointments` with `medicalCenterId`, `providerId`, and `secretaryId`.
- Backend validates provider/secretary membership within the selected medical center before booking.
- On success, backend returns calendar sync results for both doctor and secretary calendars (Google Calendar sync service scaffold).

