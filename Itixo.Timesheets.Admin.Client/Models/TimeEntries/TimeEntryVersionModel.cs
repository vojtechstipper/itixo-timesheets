using Itixo.Timesheets.Shared.Services;

namespace Itixo.Timesheets.Admin.Client.Models.TimeEntries;

public class TimeEntryVersionModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string TaskName { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public string DisplayStartTime => StartTime.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public DateTimeOffset StopTime { get; set; }
    public string DisplayStopTime => StopTime.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public string ProjectName { get; set; }
    public string Duration { get; set; }
    public DateTimeOffset LastModifiedDate { get; set; }
    public string DisplayLastModifiedDate => LastModifiedDate.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public DateTimeOffset ImportedDate { get; set; }
    public string DisplayImportedDate => ImportedDate.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public bool WasDescriptionChanged => PreviousDescription != null && Description != PreviousDescription;
    public string PreviousDescription { get; set; }
    public bool WasTaskNameChanged => PreviousTaskName != null && TaskName != PreviousTaskName;
    public string PreviousTaskName { get; set; }
    public bool WasStartTimeChanged => PreviousStartTime != default && StartTime != PreviousStartTime;
    public DateTimeOffset PreviousStartTime { get; set; }
    public bool WasStopTimeChanged => PreviousStopTime != default && StopTime != PreviousStopTime;
    public DateTimeOffset PreviousStopTime { get; set; }
    public bool WasProjectNameChanged => PreviousProjectName != null && ProjectName != PreviousProjectName;
    public string PreviousProjectName { get; set; }
}
