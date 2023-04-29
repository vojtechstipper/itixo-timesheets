using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.Invoices;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class TimeEntriesViewModel
{
    public InvoiceModalModel InvoiceModal { get; set; } = new InvoiceModalModel();

    public void PrepareSingleForInvoiceAssignment(TimeEntryGridModel timeEntry)
    {
        InvoiceModal.ToNumberFormState();
        InvoiceModal.TimeEntries.Add(InvoiceModalModel.Create(timeEntry));
    }

    public void PrepareGroupedItemForInvoiceAssignment(List<TimeEntryGridModel> timeEntries)
    {
        timeEntries.ForEach(PrepareSingleForInvoiceAssignment);
    }

    public void PrepareCheckedItemsForInvoiceAssignment()
    {
        var checkedTimeEntries = TimeEntriesGridViewDataSet.Items.Where(w => w.IsChecked).ToList();
        checkedTimeEntries.ForEach(PrepareSingleForInvoiceAssignment);
    }

    public async Task ExecuteInvoiceAssignment()
    {
        var parameter = new TimeEntriesInvoiceAssignmentParameter
        {
            Number = InvoiceModal.InvoiceNumber,
            TimeEntries = InvoiceModal.TimeEntries.Select(s => new TimeEntriesInvoiceAssignmentParameter.TimeEntry {Id = s.Id, State = s.State})
                .ToList()
        };

        ApiResult<InvoiceAssignmentResult> result = await timeEntriesApiService.AssignInvoiceAsync(parameter);

        if (result.Success)
        {
            InvoiceModal.Result = result.Value;
            InvoiceModal.ToShowResultsState();
            TimeEntriesGridViewDataSet.IsRefreshRequired = true;
        }
        else
        {
            DisplayApiResult(result.Validations);
        }
    }
}
