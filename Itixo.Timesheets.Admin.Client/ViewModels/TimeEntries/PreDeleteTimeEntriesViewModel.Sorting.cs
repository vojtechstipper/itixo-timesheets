using Itixo.Timesheets.Shared;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class PreDeleteTimeEntriesViewModel
{

    public void ApplySorting(SortExpression expression, SortDirection direction)
    {
        TimeEntriesFilter.ApplySorting(expression, direction);
        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public bool IsSortedByDescriptionAscending => IsSortingApplied(
        SortExpression.Description,
        SortDirection.Ascending);

    public bool IsSortedByDescriptionDescending => IsSortingApplied(
        SortExpression.Description,
        SortDirection.Descending);

    public bool IsSortedByProjectNameAscending => IsSortingApplied(
        SortExpression.ProjectName,
        SortDirection.Ascending);

    public bool IsSortedByProjectNameDescending => IsSortingApplied(
        SortExpression.ProjectName,
        SortDirection.Descending);

    public bool IsSortedByTaskNameAscending => IsSortingApplied(
        SortExpression.TaskName,
        SortDirection.Ascending);

    public bool IsSortedByTaskNameDescending => IsSortingApplied(
        SortExpression.TaskName,
        SortDirection.Descending);

    public bool IsSortedByDurationAscending => IsSortingApplied(
        SortExpression.Duration,
        SortDirection.Ascending);

    public bool IsSortedByDurationDescending => IsSortingApplied(
        SortExpression.Duration,
        SortDirection.Descending);

    public bool IsSortedByStartTimeAscending => IsSortingApplied(
        SortExpression.StartTime,
        SortDirection.Ascending);

    public bool IsSortedByStartTimeDescending => IsSortingApplied(
        SortExpression.StartTime,
        SortDirection.Descending);

    public bool IsSortedByStopTimeAscending => IsSortingApplied(
        SortExpression.StopTime,
        SortDirection.Ascending);

    public bool IsSortedByStopTimeDescending => IsSortingApplied(
        SortExpression.StopTime,
        SortDirection.Descending);

    public bool IsSortedByStateAscending => IsSortingApplied(
        SortExpression.State,
        SortDirection.Ascending);

    public bool IsSortedByStateDescending => IsSortingApplied(
        SortExpression.State,
        SortDirection.Descending);

    public bool IsSortedByInvoiceNumberAscending => IsSortingApplied(
        SortExpression.InvoiceNumber,
        SortDirection.Ascending);

    public bool IsSortedByInvoiceNumberDescending => IsSortingApplied(
        SortExpression.InvoiceNumber,
        SortDirection.Descending);

    public bool IsSortedByUsernameAscending => IsSortingApplied(
        SortExpression.Username,
        SortDirection.Ascending);

    public bool IsSortedByUsernameDescending => IsSortingApplied(
        SortExpression.Username,
        SortDirection.Descending);

    public bool IsSortedByCreatedDateAscending => IsSortingApplied(
        SortExpression.LastModifiedDate,
        SortDirection.Ascending);

    public bool IsSortedByExternalIdAscending => IsSortingApplied(
        SortExpression.ExternalId,
        SortDirection.Ascending);

    private bool IsSortingApplied(SortExpression expression, SortDirection direction) =>
        TimeEntriesFilter.AppliedSortings.ContainsKey(expression)
        && TimeEntriesFilter.AppliedSortings[expression] == direction;
}

