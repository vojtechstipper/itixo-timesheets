using Azure.Identity;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Routing;
using HealthChecks.UI.Client;
using Itixo.Timesheets.Admin.Client;
using Itixo.Timesheets.Admin.Client.DependencyInjection;
using Itixo.Timesheets.Admin.Client.Hubs;
using Itixo.Timesheets.Admin.Client.Messaging;
using Itixo.Timesheets.Client.Shared.DependencyInjection;
using Itixo.Timesheets.Client.Shared.Middlewares;
using Itixo.Timesheets.Shared.Messaging;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web;
using SlimMessageBus.Host.AspNetCore;
using SlimMessageBus.Host.AzureServiceBus;
using SlimMessageBus.Host.Serialization.Json;
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

builder.Services.AddAdminClient(builder.Configuration);
builder.Services.AddClientSharedLib();

builder.Services.AddDotVVM<DotvvmStartup>();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

builder.Services.AddSlimMessageBus(mbb =>
{
    mbb.WithSerializer(new JsonMessageSerializer())
       .PerMessageScopeEnabled(true)
       .WithProviderServiceBus(new ServiceBusMessageBusSettings(builder.Configuration.GetConnectionString("ServiceBus")));

    mbb.Consume<TogglSyncMessage>(x => x.Queue("autosync").WithConsumer<TogglSyncMessageConsumer>());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseStatusCodePagesWithRedirects("404");


app.UseRouting();
app.UseDatabaseAwaker();
app.UseAuthentication();

app.UseAuthorization();

// use DotVVM
DotvvmConfiguration dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(app.Environment.ContentRootPath);
dotvvmConfiguration.AssertConfigurationIsValid();

// use static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Environment.WebRootPath)
});
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TogglSyncHub>("/togglsynchub");
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();
