using System.Linq;
using System.Reflection;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using TogglSyncShared.Services;

namespace TogglSyncShared.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddTogglSyncWebJobsSharedLibs(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ICurrentIdentityProvider, CurrentIdentityProvider>();

        services.Scan(
            scan => scan
                .AddTypes(Assembly.GetAssembly(typeof(ServiceCollectionExtensions))
                    .GetTypes()
                    .Where(x => x.IsClass
                                && x.GetInterfaces().Contains(typeof(IService))))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<IApiConnectorFactory, ApiConnectorFactory>();
    }
}
