using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Itixo.Timesheets.Users.Client.Configuration;
using Itixo.Timesheets.Users.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Itixo.Timesheets.Admin.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddUserClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient<IAccountApiService, AccountApiService>(context => context.BaseAddress = new Uri(configuration["TimesheetsApiUri"]));
        services.AddScoped<IHttpClientAdAuthorizer, HttpClientAdAuthorizer>();
    }
}
