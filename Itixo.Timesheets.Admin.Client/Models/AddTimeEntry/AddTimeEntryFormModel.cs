using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Admin.Client.Models.AddTimeEntry;

public class AddTimeEntryFormModel
{
    public AddTimeEntryFormModel()
    {
        StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
        StopTime = DateTime.Now;
    }

    public string Description { get; set; }
    public string TaskName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime StopTime { get; set; }
    public int ProjectId { get; set; }
    public int TimeTrackerAccountId { get; set; }
    public bool IsApproved { get; set; }
    public List<ProjectComboItem> Projects { get; set; } = new List<ProjectComboItem>();
    public List<AccountComboItem> Accounts { get; set; } = new List<AccountComboItem>();

    public class ProjectComboItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AccountComboItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
