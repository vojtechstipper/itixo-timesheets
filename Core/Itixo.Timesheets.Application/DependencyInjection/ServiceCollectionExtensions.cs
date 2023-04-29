using Itixo.Timesheets.Application.Behaviors;
using Itixo.Timesheets.Application.TimeEntries.Services;
using Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Services;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Itixo.Timesheets.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(
            scan => scan
                .FromTypes(
                    typeof(ServiceCollectionExtensions).Assembly
                        .GetTypes()
                        .Where(
                            x => x.IsClass
                                 && x.GetInterfaces().Contains(typeof(IService))))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

        services.AddScoped(typeof(IReportTimeEntryQuery), typeof(ReportTimeEntryQuery));
        services.AddScoped(typeof(IAssignInvoiceToTimeEntriesService), typeof(AssignInvoiceToTimeEntriesService));
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(SyncSharedLockContract)));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddMediatRAttributedBehaviors(Assembly.GetExecutingAssembly());


        // TODO: attributy
        services.AddScoped(
            typeof(IPipelineBehavior<AddTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>),
            typeof(TogglSyncIdentityInfoToAccountAssigner<AddTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>));


        services.AddScoped(
            typeof(IPipelineBehavior<UpdateTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>),
            typeof(TogglSyncIdentityInfoToAccountAssigner<UpdateTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>));

        return services;
    }
}
