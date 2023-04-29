using DotVVM.Framework.Binding.Properties;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Admin.Client.Configurations;
using Itixo.Timesheets.Admin.Client.Hubs;
using Itixo.Timesheets.Admin.Client.Messaging;
using Itixo.Timesheets.Admin.Client.ViewModels;
using Itixo.Timesheets.Admin.Client.ViewModels.Base;
using Itixo.Timesheets.Admin.Client.ViewModels.Shared;
using Itixo.Timesheets.Admin.Client.ViewModels.Shared.TimeEntries;
using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Itixo.Timesheets.Shared.Messaging;
using Itixo.Timesheets.Shared.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;

namespace Itixo.Timesheets.Admin.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddAdminClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddConfiguredHttpClient<ITimeTrackerAccountsApiService, TimeTrackerAccountsApiService>(configuration);
        services.AddConfiguredHttpClient<IConfigurationsApiService, ConfigurationsApiService>(configuration);
        services.AddConfiguredHttpClient<ITimeEntriesApiService, TimeEntriesApiService>(configuration);
        services.AddConfiguredHttpClient<IReportsApiService, ReportsApiService>(configuration);
        services.AddConfiguredHttpClient<IWorkspacesApiService, WorkspacesApiService>(configuration);
        services.AddConfiguredHttpClient<ISynchronizationHistoryApiService, SynchronizationHistoryApiService>(configuration);
        services.AddConfiguredHttpClient<IAddTimeEntryApiService, AddTimeEntryApiService>(configuration);
        services.AddConfiguredHttpClient<ITimeEntryVersionsApiService, TimeEntryVersionsApiService>(configuration);
        services.AddConfiguredHttpClient<ITimeEntryStateChangesApiService, TimeEntryStateChangesApiService>(configuration);
        services.AddConfiguredHttpClient<IAccountApiService, AccountApiService>(configuration);
        services.AddConfiguredHttpClient<IPreDeletedTimeEntryApiService, PreDeletedTimeEntryApiService>(configuration);
        services.AddHttpClient<ISynchronizationApiService, SynchronizationApiService>(x => x.BaseAddress = new Uri(configuration["TimesheetsSyncApiUri"]));

        services.AddScoped<IHttpClientAdAuthorizer, HttpClientAdAuthorizer>();
        services.AddScoped<MasterPageViewModel.IDependencies, MasterPageViewModel.Dependencies>();       

        services.AddScoped<ICurrentIdentityProvider, CurrentUserProvider>();

        services.Scan(
            scan => scan
                .FromTypes(
                    typeof(ServiceCollectionExtensions).Assembly
                        .GetTypes()
                        .Where(x => x.IsClass && x.BaseType == typeof(ViewModelBase))
                )
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
            );

        services.AddTransient<IUsersFilterModel, UsersFilterModel>();
        services.AddTransient<IProjectsFilterModel, ProjectsFilterModel>();
        services.AddTransient<IClientsFilterModel, ClientsFilterModel>();
        services.AddTransient<IPager, Pager>();
    }

    public static void AddConfiguredHttpClient<TClient, TImpl>(this IServiceCollection services, IConfiguration configuration)
        where TClient : class
        where TImpl : class, TClient
    {
        services.AddHttpClient<TClient, TImpl>(context =>
        {
            context.BaseAddress = new Uri(configuration["TimesheetsApiUri"]);
        });
    }
}
