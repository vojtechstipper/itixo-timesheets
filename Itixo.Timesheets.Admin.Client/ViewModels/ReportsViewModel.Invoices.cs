using Itixo.Timesheets.Admin.Client.Models;
using Itixo.Timesheets.Admin.Client.Models.InvoiceModal;
using Itixo.Timesheets.Client.Shared;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Contracts.TimeEntries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Admin.Client.Models.Reports;
using Itixo.Timesheets.Client.Shared.ApiServices;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public partial class ReportsViewModel
{
    public InvoiceModalContext InvoiceModalContext { get; set; } = new InvoiceModalContext();

    public async Task AssignInvoice()
    {
        SummaryTimeEntriesInvoiceAssignmentParameter parameter = BuildInvoiceAssignmentParameter();

        ApiResult<InvoiceAssignmentResult> apiResult = await reportsApiService.AssignInvoiceAsync(parameter);

        if (!apiResult.Success)
        {
            DisplayApiResult(apiResult.Validations);
            InvoiceModalContext.Clear();
            return;
        }

        InvoiceModalContext.ShowResults(apiResult.Value);
    }

    private SummaryTimeEntriesInvoiceAssignmentParameter BuildInvoiceAssignmentParameter()
    {
        var usersReportsQueryFilter = new TimeTrackerAccountsReportsQueryFilter(ReportsQueryFilter)
            { TimeTrackerAccountId = InvoiceModalContext.TimeTrackerAccountId };

        AccountReportGridItemModel gridRow = ReportsGridViewDataSet.Items.First(f => f.TimeTrackerAccountId == InvoiceModalContext.TimeTrackerAccountId);

        var parameter = new SummaryTimeEntriesInvoiceAssignmentParameter(usersReportsQueryFilter)
        {
            IncludeApproved = InvoiceModalContext.IncludeApproved,
            IncludeBans = InvoiceModalContext.IncludeBans,
            IncludeDrafts = InvoiceModalContext.IncludeDrafts,
            Number = InvoiceModalContext.Number,
            TimeTrackerIds = new List<int> { InvoiceModalContext.TimeTrackerAccountId },
            SummaryDurationApproves = gridRow.SummaryDurationApprovesFormmated,
            SummaryDurationAllEntries = gridRow.SummaryDurationAllEntriesFormmated,
            SummaryDurationDrafts = gridRow.SummaryDurationDraftsFormmated,
            SummaryDurationBans = gridRow.SummaryDurationBansFormmated
        };

        return parameter;
    }
}
