# How to Start the Application (Development)

This repository currently contains production-oriented **skeletons** for backend, workstation sync components, and frontend SPA.

## Prerequisites
- Windows 10/11 (recommended for workstation modules)
- .NET SDK 8.0+
- Node.js 20+ and npm
- (Optional) Azure Storage Emulator/Azurite or Azure resources for real integration

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
- Routes are available for secretary and doctor pages.

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
