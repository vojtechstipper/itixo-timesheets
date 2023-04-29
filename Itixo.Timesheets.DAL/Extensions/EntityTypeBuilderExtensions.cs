using System;
using Itixo.Timesheets.Persistence.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itixo.Timesheets.Persistence.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> IsSoftDeleteEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        builder.Property<DateTimeOffset?>(MappingDatabaseConstants.SoftDeleteDateColumnName);
        builder.HasQueryFilter(x => EF.Property<DateTimeOffset?>(x, MappingDatabaseConstants.SoftDeleteDateColumnName) == null);
        return builder;
    }

    public static EntityTypeBuilder<TEntity> IsTimeTrackableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        builder.Property<DateTimeOffset?>(MappingDatabaseConstants.RecordsCreatedDateColumnName);
        builder.Property<DateTimeOffset?>(MappingDatabaseConstants.RecordsLastModifiedDateColumnName);
        return builder;
    }

    public static OwnedNavigationBuilder IsTimeTrackableEntity(this OwnedNavigationBuilder builder)
    {
        builder.Property<DateTimeOffset?>(MappingDatabaseConstants.RecordsCreatedDateColumnName);
        builder.Property<DateTimeOffset?>(MappingDatabaseConstants.RecordsLastModifiedDateColumnName);
        return builder;
    }
}
