using Azure.Identity;
using DotVVM.Framework.Routing;
using HealthChecks.UI.Client;
using Itixo.Timesheets.Admin.Client.DependencyInjection;
using Itixo.Timesheets.Client.Shared.DependencyInjection;
using Itixo.Timesheets.Client.Shared.Middlewares;
using Itixo.Timesheets.Shared.Services;
using Itixo.Timesheets.Users.Client;
using Itixo.Timesheets.Users.Client.Configuration;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
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

builder.Services.AddDataProtection();
builder.Services.AddAuthorization();
builder.Services.AddWebEncoders();

builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.HandleSameSiteCookieCompatibility();
});

builder.Services.AddOptions();

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(new[] { builder.Configuration["AzureAd:Scope"] })
    .AddInMemoryTokenCaches();

builder.Services.AddScoped<ICurrentIdentityProvider, CurrentUserProvider>();
builder.Services.AddClientSharedLib();
builder.Services.AddUserClient(builder.Configuration);
builder.Services.AddDotVVM<DotvvmStartup>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRouting();
app.UseDatabaseAwaker();
app.UseAuthentication();
app.UseAuthorization();

// use DotVVM
var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(app.Environment.ContentRootPath);
dotvvmConfiguration.AssertConfigurationIsValid();

// use static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Environment.WebRootPath)
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();
