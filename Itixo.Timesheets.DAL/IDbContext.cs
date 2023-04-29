using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Itixo.Timesheets.Persistence;

public interface IDbContext : IEntityFrameworkDbContext
{
    DbSet<TimeTrackerAccount> TimeTrackerAccounts { get; }
    DbSet<Project> Projects { get; }
    DbSet<Configuration> Configurations { get; }
    DbSet<Workspace> Workspaces { get; set; }
    DbSet<TimeEntry> TimeEntries { get; set; }
    DbSet<Client> Clients { get; set; }
    DbSet<Invoice> Invoices { get; set; }
    DbSet<SyncSharedLock> SyncSharedLocks { get; set; }
    DbSet<IdentityInfo> IdentityInfos { get; set; }
    DbSet<SyncBatchLogRecord> SyncBatchLogRecords { get; set; }
    DbSet<SyncLogRecord> SyncLogRecords { get; set; }
    DbSet<TimeEntryVersion> TimeEntryVersions { get; set; }
    EntityEntry Entry(object entity);
}
