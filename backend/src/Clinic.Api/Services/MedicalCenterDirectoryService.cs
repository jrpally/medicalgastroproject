using Clinic.Api.Interfaces;

namespace Clinic.Api.Services;

public sealed class MedicalCenterDirectoryService : IMedicalCenterDirectoryService
{
    private static readonly Dictionary<string, Dictionary<string, string>> CenterPersonnelCalendars = new(StringComparer.OrdinalIgnoreCase)
    {
        ["center-a"] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["doctor-1"] = "doctor-1@center-a.example.com",
            ["doctor-2"] = "doctor-2@center-a.example.com",
            ["secretary-1"] = "secretary-1@center-a.example.com"
        },
        ["center-b"] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["doctor-9"] = "doctor-9@center-b.example.com",
            ["secretary-9"] = "secretary-9@center-b.example.com"
        }
    };

    public Task EnsurePersonnelInCenterAsync(string medicalCenterId, string providerId, string secretaryId, CancellationToken ct)
    {
        if (!CenterPersonnelCalendars.TryGetValue(medicalCenterId, out var personnel))
        {
            throw new InvalidOperationException("MEDICAL_CENTER_NOT_FOUND");
        }

        if (!personnel.ContainsKey(providerId))
        {
            throw new InvalidOperationException("PROVIDER_NOT_IN_MEDICAL_CENTER");
        }

        if (!personnel.ContainsKey(secretaryId))
        {
            throw new InvalidOperationException("SECRETARY_NOT_IN_MEDICAL_CENTER");
        }

        return Task.CompletedTask;
    }

    public Task<string> ResolveCalendarIdAsync(string medicalCenterId, string userId, CancellationToken ct)
    {
        if (!CenterPersonnelCalendars.TryGetValue(medicalCenterId, out var personnel) || !personnel.TryGetValue(userId, out var calendarId))
        {
            throw new InvalidOperationException("CALENDAR_NOT_FOUND");
        }

        return Task.FromResult(calendarId);
    }
}
