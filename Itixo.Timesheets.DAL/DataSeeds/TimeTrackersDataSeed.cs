using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itixo.Timesheets.Persistence.DataSeeds;

public class TimeTrackersDataSeed : IEntityTypeConfiguration<TimeTracker>
{
    public void Configure(EntityTypeBuilder<TimeTracker> builder)
    {
        builder.HasData(new TimeTracker {Id = 1, Name = "Toggl", Type = TimeTrackerType.Toggl});
        builder.HasData(new TimeTracker {Id = 2, Name = "Tato aplikace", Type = TimeTrackerType.ThisApplication});
    }
}
