using Itixo.Timesheets.Persistence.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Itixo.Timesheets.Persistence.Extensions;

public static class DatabaseExtensions
{
    public static DbContext ClearDataOf<T>(this DbContext dbContext) where T : class
    {
        dbContext.RemoveRange(dbContext.Set<T>());
        return dbContext;
    }

    public static void UpdateSoftDeleteStatuses(AppDbContext dbContext, DateTimeOffset deletedDate)
    {
        foreach (EntityEntry entry in dbContext.ChangeTracker.Entries().Where(w => w.State == EntityState.Deleted))
        {
            if (entry.CurrentValues.Properties.Any(prop => prop.Name == MappingDatabaseConstants.SoftDeleteDateColumnName))
            {
                entry.CurrentValues[MappingDatabaseConstants.SoftDeleteDateColumnName] = deletedDate;
                entry.State = EntityState.Modified;
            }
        }
    }

    public static void UpdateCreatedDateStatuses(AppDbContext dbContext, DateTimeOffset createdDate)
    {
        foreach (EntityEntry entry in dbContext.ChangeTracker.Entries().Where(w => w.State == EntityState.Added))
        {
            if (entry.CurrentValues.Properties.Any(prop => prop.Name == MappingDatabaseConstants.RecordsCreatedDateColumnName))
            {
                entry.CurrentValues[MappingDatabaseConstants.RecordsCreatedDateColumnName] = createdDate;
            }
        }
    }

    public static void UpdateLastModifiedDateStatuses(AppDbContext dbContext, DateTimeOffset lastModifiedDate)
    {
        foreach (EntityEntry entry in dbContext.ChangeTracker.Entries().Where(w => w.State == EntityState.Modified))
        {
            if (entry.CurrentValues.Properties.Any(prop => prop.Name == MappingDatabaseConstants.RecordsLastModifiedDateColumnName))
            {
                entry.CurrentValues[MappingDatabaseConstants.RecordsLastModifiedDateColumnName] = lastModifiedDate;
            }
        }
    }
}
