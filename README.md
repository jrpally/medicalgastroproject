# medicalgastroproject

Blueprint y skeleton productivo para una plataforma clínica (medical gastro) con:
- Agenda de citas y cola en tiempo real.
- Estudios clínicos genéricos tipo EMR con adjuntos (imagen/documento/video).
- Modo offline-first (local store + outbox sync).
- Asistencia de IA para análisis de adjuntos y borradores de reporte.
- Generación determinística de reportes Word con Open XML.

## Documentación principal (links directos)
- [Guía de inicio y ejecución local](./docs/how-to-start.md)
- [Blueprint técnico completo (arquitectura, modelos, API, seguridad)](./docs/medical-clinic-blueprint.md)

## Estructura del repositorio
- [backend/src/Clinic.Api](./backend/src/Clinic.Api): API ASP.NET Core .NET 8 (REST + SignalR).
- [frontend](./frontend): SPA React + TypeScript.
- [workstation/Clinic.Workstation](./workstation/Clinic.Workstation): módulos offline (SQLite, cifrado, outbox worker) en formato skeleton.
- [scripts/local-dev.sh](./scripts/local-dev.sh): levanta backend + frontend en macOS/Linux.
- [scripts/local-dev.ps1](./scripts/local-dev.ps1): levanta backend + frontend en Windows PowerShell.

## Configuración rápida

### 1) Prerrequisitos
- .NET SDK 8+
- Node.js 20+ y npm
- (Opcional) Azurite o recursos Azure reales

### 2) Variables/configuración relevante
> Estado actual: scaffold. Varias integraciones están en modo placeholder y se deben cablear por entorno.

- **Backend Auth**: JWT Bearer habilitado, con políticas por rol (`Administrator`, `Secretary`, `Doctor`).
- **Calendario**: existe un servicio `GoogleCalendarSyncService` de tipo scaffold (no OAuth productivo todavía).
- **Storage/Queue**: hay abstracciones Azure (Tables/Blob/Queue) listas para implementar wiring real.

### 3) Arranque local (recomendado)
- **Windows PowerShell**:
  ```powershell
  ./scripts/local-dev.ps1
  ```
- **macOS/Linux**:
  ```bash
  ./scripts/local-dev.sh
  ```

Para instrucciones detalladas paso a paso, revisa:
- [docs/how-to-start.md](./docs/how-to-start.md)

## Rutas frontend de demo (scaffold)
- `/admin/users`
- `/secretary/calendar`
- `/doctor/worklist`
- `/doctor/patients/:id/timeline`
- `/doctor/studies/:studyId`
- `/doctor/reports/:draftId`

## Estado del proyecto
Este repositorio está en modo **starter/skeleton**: sirve para validar arquitectura, contratos, flujo base y estructura de módulos, pero aún requiere integración productiva final (infra, OAuth Google real, persistencia completa, tests integrados end-to-end).
