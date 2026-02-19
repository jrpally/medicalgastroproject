namespace Clinic.Api.DTOs;

public sealed record ApiErrorResponse(string Code, string Message, IReadOnlyCollection<ApiErrorDetail> Details, string CorrelationId);

public sealed record ApiErrorDetail(string Field, string Issue);

public sealed record SlotDto(string ProviderId, DateTimeOffset StartUtc, DateTimeOffset EndUtc, bool IsFree, string? Rule);

public sealed record CalendarSyncStatus(string UserId, string CalendarId, string EventId, string Status);

public sealed record CreateAppointmentResponse(Guid AppointmentId, string MedicalCenterId, string Status, IReadOnlyCollection<CalendarSyncStatus> CalendarSync);

public sealed record TimelineResponse(
    string PatientId,
    IReadOnlyCollection<object> Appointments,
    IReadOnlyCollection<object> Encounters,
    IReadOnlyCollection<object> Studies,
    IReadOnlyCollection<object> Attachments);
