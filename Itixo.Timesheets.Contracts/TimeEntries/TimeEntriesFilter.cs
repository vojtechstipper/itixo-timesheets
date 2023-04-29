using Itixo.Timesheets.Shared;
using Itixo.Timesheets.Shared.Converters;
using Itixo.Timesheets.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Contracts.TimeEntries;

public class TimeEntriesFilter
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = int.MaxValue;
    public bool IsDraftedRequired { get; set; }
    public bool IsBannedRequired { get; set; }
    public bool IsApprovedRequired { get; set; }
    public bool IsAllIncludingChildrenRequired { get; set; }
    public bool IsPreDeleteRequired { get; set; }
    public DateTime FromDate { get; set; } = default;
    public DateTime ToDate { get; set; } = default;
    public List<TimeEntryInvoiceState> SelectedInvoiceStates { get; set; } = new List<TimeEntryInvoiceState>();
    public List<int> AccountIds { get; set; } = new List<int>();
    public List<int> ProjectIds { get; set; } = new List<int>();
    public List<int> ClientIds { get; set; } = new List<int>();
    public List<string> IncludingChildrenGroupIds { get; set; } = new List<string>();
    public string ProjectNameSearchText { get; set; } = "";
    public string TaskNameSearchText { get; set; } = "";
    public string DescriptionSearchText { get; set; } = "";
    public string UsernameSearchText { get; set; } = "";
    public string InvoiceNumberSearchText { get; set; } = "";

    [System.Text.Json.Serialization.JsonConverter(typeof(SortExpressionConverter))]
    public Dictionary<SortExpression, SortDirection> AppliedSortings { get; set; } = new Dictionary<SortExpression, SortDirection>();

    public void ApplySorting(SortExpression expression, SortDirection direction)
    {
        if (AppliedSortings.ContainsKey(expression))
        {
            AppliedSortings[expression] = direction;
        }
        else
        {
            AppliedSortings.Add(expression, direction);
        }
    }

    public static TimeEntriesFilter Clone(TimeEntriesFilter filter)
    {
        return new TimeEntriesFilter
        {
            ProjectIds = filter.ProjectIds,
            ClientIds = filter.ClientIds,
            FromDate = filter.FromDate,
            DescriptionSearchText = filter.DescriptionSearchText,
            IsApprovedRequired = filter.IsApprovedRequired,
            IsBannedRequired = filter.IsBannedRequired,
            IsDraftedRequired = filter.IsDraftedRequired,
            IsPreDeleteRequired = filter.IsPreDeleteRequired,
            ProjectNameSearchText = filter.ProjectNameSearchText,
            UsernameSearchText = filter.UsernameSearchText,
            InvoiceNumberSearchText = filter.InvoiceNumberSearchText,
            ToDate = filter.ToDate,
            TaskNameSearchText = filter.TaskNameSearchText,
            AccountIds = filter.AccountIds,
            Skip = filter.Skip,
            AppliedSortings = filter.AppliedSortings,
            Take = filter.Take,
            IncludingChildrenGroupIds = filter.IncludingChildrenGroupIds,
            IsAllIncludingChildrenRequired = filter.IsAllIncludingChildrenRequired,
            SelectedInvoiceStates = filter.SelectedInvoiceStates
        };
    }
}
