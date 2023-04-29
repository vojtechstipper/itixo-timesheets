using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Itixo.Timesheets.Notificator.Lib;
using Itixo.Timesheets.Shared.Messaging;
using Itixo.Timesheets.SyncApi.Commands;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Identity.Web;
using SlimMessageBus.Host.AspNetCore;
using SlimMessageBus.Host.AzureServiceBus;
using SlimMessageBus.Host.Serialization.Json;
using System.Reflection;
using TogglSyncShared.DependencyInjection;

var builder = WebApplication.CreateBuilder();

builder.Configuration
    .AddJsonFile(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Roles", "AppRoles.json"), optional: true)
    .AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultUri"]), new DefaultAzureCredential());

if (builder.Environment.IsProduction())
{
    builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions() { ConnectionString = builder.Configuration.GetConnectionString("AppInsights") });
}

builder.Services.AddHealthChecks();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddTogglSyncWebJobsSharedLibs();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, subscribeToJwtBearerMiddlewareDiagnosticsEvents: true);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RunSynchronizationCommandValidator>();

builder.Services.AddSlimMessageBus(mbb =>
{
    mbb.WithSerializer(new JsonMessageSerializer())
       .PerMessageScopeEnabled(true)
       .WithProviderServiceBus(new ServiceBusMessageBusSettings(builder.Configuration.GetConnectionString("ServiceBus")));

    mbb.Produce<TogglSyncMessage>(x => x.DefaultQueue("autosync"));
    mbb.Produce<NotificatorMessage>(x => x.DefaultQueue("notification"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();
