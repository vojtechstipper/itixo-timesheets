using DotVVM.Framework.ViewModel;
using System;
using System.Linq;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class PreDeleteTimeEntriesViewModel
{
    public const string UserIdsParamName = "uids";
    public const string FromDateParamName = "from";
    public const string ToDateParamName = "to";
    public const string ProjectIdsParamName = "pids";
    public const string ClientIdsParamName = "cids";
    public const string ProjectSearchParamName = "ps";
    public const string TaskNameSearchParamName = "tns";
    public const string DescriptionSearchParamName = "ds";
    public const string UsernameSearchParamName = "us";

    [FromQuery(UserIdsParamName)]
    public string UserIds { get; set; } = "";

    [FromQuery(FromDateParamName)]
    public DateTime FromDateQueryParam { get; set; } = default;

    [FromQuery(ToDateParamName)]
    public DateTime ToDateQueryParam { get; set; } = default;

    [FromQuery(ProjectIdsParamName)]
    public string ProjectIds { get; set; } = "";

    [FromQuery(ClientIdsParamName)]
    public string ClientIds { get; set; } = "";

    [FromQuery(ProjectSearchParamName)]
    public string ProjectNameSearchText { get; set; } = "";

    [FromQuery(TaskNameSearchParamName)]
    public string TaskNameSearchText { get; set; } = "";

    [FromQuery(DescriptionSearchParamName)]
    public string DescriptionSearchText { get; set; } = "";

    [FromQuery(UsernameSearchParamName)]
    public string UsernameSearchText { get; set; } = "";


    public string ProjectIdsQs => ProjectsFilterModel.SelectedProjects.Any() ? string.Join(",", ProjectsFilterModel.SelectedProjects.Select(project => project.Id)) : default;
    public string ClientIdsQs => ClientsFilterModel.SelectedClients.Any() ? string.Join(",", ClientsFilterModel.SelectedClients.Select(client => client.Id)) : default;
    public string UserIdsQs => UsersFilterModel.SelectedUsers.Any() ? string.Join(",", UsersFilterModel.SelectedUsers.Select(user => user.Id)) : default;
    public DateTime FromDateQs => TimeEntriesFilter.FromDate;
    public DateTime ToDateQs => TimeEntriesFilter.ToDate;

    private void LoadQueryParamsIntoFilter()
    {
        if (FromDateQueryParam != default)
        {
            TimeEntriesFilter.FromDate = FromDateQueryParam;
        }

        if (ToDateQueryParam != default)
        {
            TimeEntriesFilter.ToDate = ToDateQueryParam;
        }

        if (!string.IsNullOrWhiteSpace(ClientIds))
        {
            TimeEntriesFilter.ClientIds = ClientIds.Split(",").Select(int.Parse).ToList();
            ClientsFilterModel.SelectedClients = ClientsFilterModel.Clients.Where(w => TimeEntriesFilter.ClientIds.Contains(w.Id)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(ProjectIds))
        {
            TimeEntriesFilter.ProjectIds = ProjectIds.Split(",").Select(int.Parse).ToList();
            ProjectsFilterModel.SelectedProjects = ProjectsFilterModel.Projects.Where(w => TimeEntriesFilter.ProjectIds.Contains(w.Id)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(UserIds))
        {
            TimeEntriesFilter.AccountIds = UserIds.Split(",").Select(int.Parse).ToList();
            UsersFilterModel.SelectedUsers = UsersFilterModel.Users.Where(w => TimeEntriesFilter.AccountIds.Contains(w.Id)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(ProjectNameSearchText))
        {
            TimeEntriesFilter.ProjectNameSearchText = ProjectNameSearchText;
        }

        if (!string.IsNullOrWhiteSpace(DescriptionSearchText))
        {
            TimeEntriesFilter.DescriptionSearchText = DescriptionSearchText;
        }

        if (!string.IsNullOrWhiteSpace(TaskNameSearchText))
        {
            TimeEntriesFilter.TaskNameSearchText = TaskNameSearchText;
        }

        if (!string.IsNullOrWhiteSpace(UsernameSearchText))
        {
            TimeEntriesFilter.UsernameSearchText = UsernameSearchText;
        }

    }
}
