using DotVVM.Framework.ViewModel;
using Itixo.Timesheets.Contracts.TimeEntries;
using System;
using System.Linq;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class TimeEntriesViewModel
{
    public const string DraftsParamName = "d";
    public const string BansParamName = "b";
    public const string ApprovesParamName = "a";
    public const string UserIdsParamName = "uids";
    public const string FromDateParamName = "from";
    public const string ToDateParamName = "to";
    public const string InvoiceAssignedParamName = "ia";
    public const string ProjectIdsParamName = "pids";
    public const string ClientIdsParamName = "cids";
    public const string ProjectSearchParamName = "ps";
    public const string TaskNameSearchParamName = "tns";
    public const string DescriptionSearchParamName = "ds";
    public const string UsernameSearchParamName = "us";
    public const string InvoiceNumberSearchParamName = "ins";
    public const string IncludeVersionParamName = "versions";
    public const string InvoiceStateParamName = "invs";

    [FromQuery(DraftsParamName)]
    public bool? IsDraftedRequired { get; set; }

    [FromQuery(BansParamName)]
    public bool? IsBannedRequired { get; set; }

    [FromQuery(ApprovesParamName)]
    public bool? IsApprovedRequired { get; set; }

    [FromQuery(UserIdsParamName)]
    public string UserIds { get; set; } = "";

    [FromQuery(FromDateParamName)]
    public DateTime FromDateQueryParam { get; set; } = default;

    [FromQuery(ToDateParamName)]
    public DateTime ToDateQueryParam { get; set; } = default;

    [FromQuery(InvoiceAssignedParamName)]
    public bool? IsInvoiceAssigned { get; set; }

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

    [FromQuery(InvoiceNumberSearchParamName)]
    public string InvoiceNumberSearchText { get; set; } = "";

    [FromQuery(IncludeVersionParamName)]
    public bool? IncludeVersion { get; set; }

    [FromQuery(InvoiceStateParamName)]
    public string InvoiceState { get; set; } = "";

    public string ProjectIdsQs => ProjectsFilterModel.SelectedProjects.Any() ? string.Join(",", ProjectsFilterModel.SelectedProjects.Select(project => project.Id)) : default;
    public string ClientIdsQs => ClientsFilterModel.SelectedClients.Any() ? string.Join(",", ClientsFilterModel.SelectedClients.Select(client => client.Id)) : default;
    public string UserIdsQs => UsersFilterModel.SelectedUsers.Any() ? string.Join(",", UsersFilterModel.SelectedUsers.Select(user => user.Id)) : default;
    public string IsDraftedQs => TimeEntriesFilter.IsDraftedRequired ? bool.TrueString : default;
    public string IsBannedQs => TimeEntriesFilter.IsBannedRequired ? bool.TrueString : default;
    public string IsApprovedQs => TimeEntriesFilter.IsApprovedRequired ? bool.TrueString : default;
    public DateTime FromDateQs => TimeEntriesFilter.FromDate;
    public DateTime ToDateQs => TimeEntriesFilter.ToDate;
    public string InvoiceStateIdsQs => SelectedInvoiceStates.Any() ? string.Join(",", SelectedInvoiceStates.Select(invState => invState.TimeEntryInvoiceState)) : default;

    private void LoadQueryParamsIntoFilter()
    {
        if (IsBannedRequired != null)
        {
            TimeEntriesFilter.IsBannedRequired = IsBannedRequired.Value;
            SelectedStates.Add(FilteredQueryTimeEntryItemContractBase.BanState);
        }

        if (IsDraftedRequired != null)
        {
            TimeEntriesFilter.IsDraftedRequired = IsDraftedRequired.Value;
            SelectedStates.Add(FilteredQueryTimeEntryItemContractBase.DraftState);
        }

        if (IsApprovedRequired != null)
        {
            TimeEntriesFilter.IsApprovedRequired = IsApprovedRequired.Value;
            SelectedStates.Add(FilteredQueryTimeEntryItemContractBase.ApprovedState);
        }

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

        if (!string.IsNullOrWhiteSpace(InvoiceNumberSearchText))
        {
            TimeEntriesFilter.InvoiceNumberSearchText = InvoiceNumberSearchText;
        }

        if (!string.IsNullOrWhiteSpace(DescriptionSearchText))
        {
            TimeEntriesFilter.DescriptionSearchText = DescriptionSearchText;
        }

        if (!string.IsNullOrWhiteSpace(TaskNameSearchText))
        {
            TimeEntriesFilter.TaskNameSearchText = TaskNameSearchText;
        }

        if (!string.IsNullOrWhiteSpace(InvoiceNumberSearchText))
        {
            TimeEntriesFilter.InvoiceNumberSearchText = InvoiceNumberSearchText;
        }

        if (!string.IsNullOrWhiteSpace(UsernameSearchText))
        {
            TimeEntriesFilter.UsernameSearchText = UsernameSearchText;
        }

        if (IncludeVersion != null)
        {
            TimeEntriesFilter.IsAllIncludingChildrenRequired = IncludeVersion.Value;
        }

        if (!string.IsNullOrWhiteSpace(InvoiceState))
        {
            TimeEntriesFilter.SelectedInvoiceStates = InvoiceStates.Select(x => x.TimeEntryInvoiceState).Where(x => InvoiceState.Split(",").Contains(x.ToString())).ToList();
            SelectedInvoiceStates = InvoiceStates.Where(state => TimeEntriesFilter.SelectedInvoiceStates.Contains(state.TimeEntryInvoiceState)).ToList();
        }
    }
}
