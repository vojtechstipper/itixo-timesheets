using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Shared.Services;
using System.Collections.Generic;
using System.Linq;
using TogglSyncShared.DataObjects;

namespace TogglSyncShared.InvalidRecords;

public class ReportersInvalidRecord
{
    public string Description { get; set; }
    public string StartTime { get; set; }
    public string StopTime { get; set; }
    public string ProjectName { get; set; }
    public string WhyIsItInvalid { get; set; }
    public long ExternalId { get; set; }

    public static List<ReportersInvalidRecord> CreateListFrom(List<TogglTimeEntry> togglTimeEntries, List<ProjectContract> projects)
    {
        var records = new List<ReportersInvalidRecord>();
        if (togglTimeEntries is not null)
        {
            foreach (TogglTimeEntry togglTimeEntry in togglTimeEntries.Where(x => x.IsTrackingFinished()))
            {
                ProjectContract project = projects.FirstOrDefault(f => f.ExternalId == togglTimeEntry.ProjectId);
                ReportersInvalidRecord record = CreateFrom(togglTimeEntry, project);

                if (record is not null)
                {
                    records.Add(record);
                }
            }
        }
        return records;
    }

    public static ReportersInvalidRecord CreateFrom(TogglTimeEntry togglTimeEntry, ProjectContract project)
    {
        if (togglTimeEntry.IsProjectInvalid())
        {
            return new ReportersInvalidRecord
            {
                Description = togglTimeEntry.Description,
                StartTime = togglTimeEntry.Start?.ToString(CetDateConversionHelper.DisplayFormat) ?? "",
                StopTime = togglTimeEntry.Stop?.ToString(CetDateConversionHelper.DisplayFormat) ?? "",
                WhyIsItInvalid = "Chybí vyplnění projektu.",
                ExternalId = togglTimeEntry.Id ?? 0
            };
        }

        if (togglTimeEntry.IsDescriptionEmpty())
        {
            return new ReportersInvalidRecord
            {
                ProjectName = project?.Name ?? "",
                StartTime = togglTimeEntry.Start?.ToString(CetDateConversionHelper.DisplayFormat) ?? "",
                StopTime = togglTimeEntry.Stop?.ToString(CetDateConversionHelper.DisplayFormat) ?? "",
                WhyIsItInvalid = "Chybí vyplnění popisu.",
                ExternalId = togglTimeEntry.Id ?? 0
            };
        }
        return null;
    }
}
