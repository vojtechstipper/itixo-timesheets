using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Itixo.Timesheets.Shared.Extensions;

public static class HostExtensions
{
    public static WebApplication EnableAutomigrations(this WebApplication host)
    {
        using IServiceScope scope = host.Services.CreateScope();

        try
        {
            IEntityFrameworkDbContext context = scope.ServiceProvider.GetService<IEntityFrameworkDbContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            ILogger<IEntityFrameworkDbContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<IEntityFrameworkDbContext>>();
            logger.LogError(ex, "An error occurred while migrating or initializing the database.");
        }

        return host;
    }
}
