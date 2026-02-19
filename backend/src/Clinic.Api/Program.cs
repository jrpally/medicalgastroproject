using Clinic.Api.Hubs;
using Clinic.Api.Interfaces;
using Clinic.Api.Repositories;
using Clinic.Api.Services;
using Clinic.Api.Storage;
using Clinic.Api.Security;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IAiService, AiService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IMedicalCenterDirectoryService, MedicalCenterDirectoryService>();
builder.Services.AddScoped<IGoogleCalendarSyncService, GoogleCalendarSyncService>();

builder.Services.AddScoped<IAppointmentRepository, TableAppointmentRepository>();
builder.Services.AddScoped<IStudyRepository, TableStudyRepository>();
builder.Services.AddScoped<ITableClientFactory, TableClientFactory>();
builder.Services.AddScoped<IBlobStorageClient, BlobStorageClient>();
builder.Services.AddScoped<IQueuePublisher, QueuePublisher>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
    options.TokenValidationParameters.NameClaimType = "name";
});

builder.Services.AddTransient<IClaimsTransformation, RolesClaimsTransformation>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.AdministratorOnly, policy => policy.RequireRole(AuthRoles.Administrator));
    options.AddPolicy(AuthorizationPolicies.SecretaryOrAdministrator, policy => policy.RequireRole(AuthRoles.Secretary, AuthRoles.Administrator));
    options.AddPolicy(AuthorizationPolicies.DoctorOrAdministrator, policy => policy.RequireRole(AuthRoles.Doctor, AuthRoles.Administrator));
    options.AddPolicy(AuthorizationPolicies.ClinicalStaff, policy => policy.RequireRole(AuthRoles.Doctor, AuthRoles.Secretary, AuthRoles.Administrator));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ClinicEventsHub>("/hubs/events");

app.Run();
