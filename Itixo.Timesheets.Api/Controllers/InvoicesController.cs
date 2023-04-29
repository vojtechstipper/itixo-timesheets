using Itixo.Timesheets.Application.Invoices.AssignTimeEntriesToInvoiceCommand;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
public class InvoicesController : AppControllerBase
{
    private readonly IInvoiceAssignmentProcessor invoiceAssignmentProcessor;

    public InvoicesController(IInvoiceAssignmentProcessor invoiceAssignmentProcessor)
    {
        this.invoiceAssignmentProcessor = invoiceAssignmentProcessor;
    }

    [HttpPost]
    public async Task<IActionResult> AssignInvoiceToSummaries([FromBody] SummaryTimeEntriesInvoiceAssignmentParameter parameter)
    {
        parameter.FromDate = parameter.FromDate.GetDateWithMinimumTime();
        parameter.ToDate = parameter.ToDate.GetDateWithMaximumTime();

        InvoiceAssignmentResult result = await invoiceAssignmentProcessor.ProcessAsync(parameter);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AssignInvoiceToTimeEntries([FromBody] TimeEntriesInvoiceAssignmentParameter parameter)
    {
        InvoiceAssignmentResult result = await invoiceAssignmentProcessor.ProcessAsync(parameter);
        return Ok(result);
    }
}
