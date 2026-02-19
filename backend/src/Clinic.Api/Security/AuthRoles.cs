namespace Clinic.Api.Security;

public static class AuthRoles
{
    public const string Administrator = "Administrator";
    public const string Secretary = "Secretary";
    public const string Doctor = "Doctor";
}

public static class AuthorizationPolicies
{
    public const string AdministratorOnly = nameof(AdministratorOnly);
    public const string SecretaryOrAdministrator = nameof(SecretaryOrAdministrator);
    public const string DoctorOrAdministrator = nameof(DoctorOrAdministrator);
    public const string ClinicalStaff = nameof(ClinicalStaff);
}
