using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Contracts.TimeEntries.Interfaces;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Enums;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery.GroupedQuery;

public interface IFilteredTimeEntriesGrouper : IService
{
    IEnumerable<FilteredTimeEntryItemContract> GetGroupedTimeEntries(IEnumerable<FilteredTimeEntryItemContract> timeEntries, TimeEntriesFilter filter);
}

public class FilteredTimeEntriesGrouper : IFilteredTimeEntriesGrouper
{
    private readonly List<FilteredTimeEntryItemContract> results = new List<FilteredTimeEntryItemContract>();
    private TimeEntriesFilter filter;

    public IEnumerable<FilteredTimeEntryItemContract> GetGroupedTimeEntries(
        IEnumerable<FilteredTimeEntryItemContract> timeEntries,
        TimeEntriesFilter filter)
    {
        this.filter = filter;

        var timeEntryGroups = Group(timeEntries);

        foreach (var timeEntryGroup in timeEntryGroups)
        {
            IEnumerable<FilteredTimeEntryItemContract> groupSubItems = timeEntryGroup.OrderBy(x => x.StartTime).ToList();

            if (groupSubItems.Count() == 1)
            {
                AddGroupToResults(groupSubItems.First(), groupSubItems.ToList());
                continue;
            }

            FilteredTimeEntryItemContract groupsHeaderItem = CreateGroupHeaderItem(timeEntryGroup, groupSubItems.ToList());
            AddGroupToResults(groupsHeaderItem, timeEntryGroup.ToList());
        }

        return results;
    }

    public static List<IGrouping<(string Description, string ProjectName, int UserId, int ProjectId, string TaskName, string Username), T>>
        Group<T>(IEnumerable<T> timeEntries)
        where T : IFilteredTimeEntriesContract
    {
        return timeEntries.GroupBy(
                timeEntry => (
                    timeEntry.Description,
                    timeEntry.ProjectName,
                    UserId: timeEntry.TimeTrackerAccountId,
                    timeEntry.ProjectId,
                    timeEntry.TaskName,
                    timeEntry.Username
                ),
                timeEntry => timeEntry)
            .ToList();
    }

    private void AddGroupToResults(FilteredTimeEntryItemContract groupsHeaderItem, List<FilteredTimeEntryItemContract> groupSubItems)
    {
        bool includeSubItemsToResults = groupsHeaderItem != null && filter.IncludingChildrenGroupIds.Contains(groupsHeaderItem.GroupId) || (filter.IsAllIncludingChildrenRequired && groupsHeaderItem?.ExternalId == null);

        if (!filter.SelectedInvoiceStates.Any())
        {
            Add(groupsHeaderItem, groupSubItems);
        }

        if (filter.SelectedInvoiceStates.Contains(TimeEntryInvoiceState.AllInvoiceStates))
        {
            Add(groupsHeaderItem, groupSubItems);
        }

        if (filter.SelectedInvoiceStates.Contains(TimeEntryInvoiceState.PartlyInvoiced))
        {
            if (groupSubItems.Select(s => s.InvoiceNumber).Distinct().Count() > 1 && groupSubItems.Any(x => x.InvoiceNumber == null))
            {
                Add(groupsHeaderItem, groupSubItems);
            }
        }

        if (filter.SelectedInvoiceStates.Contains(TimeEntryInvoiceState.OnlyInvoiced))
        {
            var additionItems = groupSubItems.Where(w => !string.IsNullOrWhiteSpace(w.InvoiceNumber)).ToList();

            if (additionItems.Any())
            {
                Add(groupsHeaderItem, additionItems);
            }
        }

        if (filter.SelectedInvoiceStates.Contains(TimeEntryInvoiceState.OnlyUninvoiced))
        {
            var additionItems = groupSubItems.Where(w => string.IsNullOrWhiteSpace(w.InvoiceNumber)).ToList();

            if (additionItems.Any())
            {
                Add(groupsHeaderItem, additionItems);
            }
        }

        void Add(FilteredTimeEntryItemContract headerItem, List<FilteredTimeEntryItemContract> subItems)
        {
            if (headerItem != null)
            {
                AddWhenNotInYet(headerItem);
            }

            if (includeSubItemsToResults && subItems.Any())
            {
                AddWhenNotInYet(subItems);
            }
        }
    }

    private void AddWhenNotInYet(IEnumerable<FilteredTimeEntryItemContract> itemsInGroup)
    {
        foreach (FilteredTimeEntryItemContract item in itemsInGroup)
        {
            AddWhenNotInYet(item);
        }
    }

    private void AddWhenNotInYet(FilteredTimeEntryItemContract item)
    {
        if (!results.Contains(item))
        {
            results.Add(item);
        }
    }

    private static FilteredTimeEntryItemContract CreateGroupHeaderItem(IGrouping<(string Description, string ProjectName, int UserId, int ProjectId, string TaskName, string Username), FilteredTimeEntryItemContract> group, List<FilteredTimeEntryItemContract> groupSubitems)
    {
        var groupsHeaderItem = new FilteredTimeEntryItemContract
        {
            Description = group.Key.Description,
            ProjectId = group.Key.ProjectId,
            TimeTrackerAccountId = group.Key.UserId,
            Username = group.Key.Username,
            ProjectName = group.Key.ProjectName,
            TaskName = group.Key.TaskName,
            LastModifiedDate = groupSubitems.First().LastModifiedDate,
            StartTime = groupSubitems.First().StartTime,
            StopTime = groupSubitems.Last().StopTime,
            Duration = new TimeSpan(groupSubitems.Sum(x => x.Duration.FromCustomHoursFormat().Ticks)).ToCustomHoursFormat(),
            GroupId = groupSubitems.Count() > 1 ? string.Join("#", group.Select(x => x.ExternalId)) : "",
            ExternalId = groupSubitems.Count() == 1 ? groupSubitems.First().ExternalId : null,
            IsBan = groupSubitems.Count() == 1 && groupSubitems.First().IsBan,
            IsDraft = groupSubitems.Count() == 1 && groupSubitems.First().IsDraft,
            IsApproved = groupSubitems.Count() == 1 && groupSubitems.First().IsApproved
        };
        return groupsHeaderItem;
    }
}
