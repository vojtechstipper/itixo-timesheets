using Azure.Identity;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Itixo.Timesheets.Application.DependencyInjection;
using Itixo.Timesheets.Application.Services.Shared;
using Itixo.Timesheets.Infrastructure.DependencyInjection;
using Itixo.Timesheets.Shared.ErrorHandling;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Identity.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder();

builder.Logging.AddConsole();

builder.Configuration.AddJsonFile(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Roles", "AppRoles.json"), optional: true)
                     .AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultUri"]), new DefaultAzureCredential());

if (builder.Environment.IsProduction())
{
    builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions() { ConnectionString = builder.Configuration.GetConnectionString("AppInsights") });
}

builder.Services.AddScoped<ICurrentIdentityProvider, CurrentIdentityProvider>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, subscribeToJwtBearerMiddlewareDiagnosticsEvents: true);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddHealthChecks()
                .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer"));


var app = builder.Build();

app.EnableAutomigrations();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.ConfigureExceptionHandler();
app.UseCookiePolicy();
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
