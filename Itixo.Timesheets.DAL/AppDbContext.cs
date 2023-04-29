using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Persistence.DataSeeds;
using Itixo.Timesheets.Persistence.EntityTypeConfigurations;
using Itixo.Timesheets.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Persistence;

public class AppDbContext : DbContext, IDbContext
{
    public virtual DbSet<TimeTrackerAccount> TimeTrackerAccounts { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Configuration> Configurations { get; set; }
    public virtual DbSet<Workspace> Workspaces { get; set; }
    public virtual DbSet<TimeEntry> TimeEntries { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<SyncSharedLock> SyncSharedLocks { get; set; }
    public virtual DbSet<IdentityInfo> IdentityInfos { get; set; }
    public virtual DbSet<SyncLogRecord> SyncLogRecords { get; set; }
    public virtual DbSet<SyncBatchLogRecord> SyncBatchLogRecords { get; set; }
    public virtual DbSet<TimeEntryVersion> TimeEntryVersions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TimeTrackerAccountConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new SyncSharedLockConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new ConfigurationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityInfoConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityTimeTrackerAccountConfiguration());
        modelBuilder.ApplyConfiguration(new SyncLogRecordConfiguration());
        modelBuilder.ApplyConfiguration(new SyncBatchLogRecordConfiguration());
        modelBuilder.ApplyConfiguration(new TimeEntryConfiguration());
        modelBuilder.ApplyConfiguration(new TimeEntryVersionConfiguration());
        modelBuilder.ApplyConfiguration(new TimeTrackerConfiguration());
        modelBuilder.ApplyConfiguration(new InvalidTimeEntriesReportConfiguration());

        modelBuilder.ApplyConfiguration(new TimeTrackersDataSeed());
    }

    public override int SaveChanges()
    {
        DatabaseExtensions.UpdateSoftDeleteStatuses(this, DateTimeOffset.Now);
        DatabaseExtensions.UpdateCreatedDateStatuses(this, DateTimeOffset.Now);
        DatabaseExtensions.UpdateLastModifiedDateStatuses(this, DateTimeOffset.Now);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DatabaseExtensions.UpdateSoftDeleteStatuses(this, DateTimeOffset.Now);
        DatabaseExtensions.UpdateCreatedDateStatuses(this, DateTimeOffset.Now);
        DatabaseExtensions.UpdateLastModifiedDateStatuses(this, DateTimeOffset.Now);
        return base.SaveChangesAsync(cancellationToken);
    }
}
