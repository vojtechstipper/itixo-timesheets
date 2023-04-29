using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Infrastructure.FilteredQueries;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itixo.Timesheets.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<AppDbContext>(configuration.GetConnectionString("SqlServer"), options =>
                options.EnableRetryOnFailure(5)
            );

        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
        services.AddScoped<IEntityFrameworkDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

        services.AddScoped(typeof(IPersistenceQuery<,>), typeof(BasicEntityQuery<,>));
        services.AddScoped<IPersistenceQuery<TimeEntry, int>, AuthorizationSecuredTimeEntriesQuery>();
        services.AddScoped<IPersistenceQuery<TimeTrackerAccount, int>, AuthorizationSecuredAccountsQuery>();

        services.Scan(
            scan => scan.FromCallingAssembly()
                .AddClasses(classes => classes.InNamespaces(
                    "Itixo.Timesheets.Infrastructure.Repos"))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
    }
}
