using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Application.TimeEntries.Services;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Invoices.AssignTimeEntriesToInvoiceCommand;

public interface IInvoiceAssignmentProcessor : IService
{
    Task<InvoiceAssignmentResult> ProcessAsync(SummaryTimeEntriesInvoiceAssignmentParameter parameter);
    Task<InvoiceAssignmentResult> ProcessAsync(TimeEntriesInvoiceAssignmentParameter parameter);
}

public class InvoiceAssignmentProcessor : IInvoiceAssignmentProcessor
{
    private readonly IInvoiceRepository invoiceRepository;
    public readonly IAssignInvoiceToTimeEntriesService assignInvoiceToTimeEntriesService;

    public InvoiceAssignmentProcessor(
        IInvoiceRepository invoiceRepository, IAssignInvoiceToTimeEntriesService assignInvoiceToTimeEntriesService)
    {
        this.invoiceRepository = invoiceRepository;
        this.assignInvoiceToTimeEntriesService = assignInvoiceToTimeEntriesService;
    }

    public async Task<InvoiceAssignmentResult> ProcessAsync(SummaryTimeEntriesInvoiceAssignmentParameter parameter)
    {
        Invoice invoice = await invoiceRepository.GetOrCreateInvoice(parameter.Number);

        var processorResult = new InvoiceAssignmentResult { InvoiceNumber = parameter.Number };
        if (parameter.IncludeApproved)
        {
            AssignInvoiceResult result = await assignInvoiceToTimeEntriesService.AssignApproves(invoice, parameter);
            processorResult.ApprovedTimeEntriesCount = result.AssignedTimeEntriesCount;
        }

        if (parameter.IncludeDrafts)
        {
            AssignInvoiceResult result = await assignInvoiceToTimeEntriesService.AssignDrafts(invoice, parameter);
            processorResult.DraftedTimeEntriesCount = result.AssignedTimeEntriesCount;
        }

        if (parameter.IncludeBans)
        {
            AssignInvoiceResult result = await assignInvoiceToTimeEntriesService.AssignBans(invoice, parameter);
            processorResult.BannedTimeEntriesCount = result.AssignedTimeEntriesCount;
        }

        return processorResult;
    }

    public async Task<InvoiceAssignmentResult> ProcessAsync(TimeEntriesInvoiceAssignmentParameter parameter)
    {
        Invoice invoice = await invoiceRepository.GetOrCreateInvoice(parameter.Number);

        var processorResult = new InvoiceAssignmentResult { InvoiceNumber = parameter.Number };

        AssignInvoiceResult timeEntriesAssignInvoiceResult = await assignInvoiceToTimeEntriesService
            .Assign(invoice, parameter.TimeEntries.Where(w => w.State == FilteredQueryTimeEntryItemContractBase.ApprovedState).Select(s => s.Id));
        processorResult.ApprovedTimeEntriesCount = timeEntriesAssignInvoiceResult.AssignedTimeEntriesCount;

        AssignInvoiceResult draftsAssignInvoiceResult = await assignInvoiceToTimeEntriesService
            .Assign(invoice, parameter.TimeEntries.Where(w => w.State == FilteredQueryTimeEntryItemContractBase.DraftState).Select(s => s.Id));
        processorResult.DraftedTimeEntriesCount = draftsAssignInvoiceResult.AssignedTimeEntriesCount;

        AssignInvoiceResult bansAssignInvoiceResult = await assignInvoiceToTimeEntriesService
            .Assign(invoice, parameter.TimeEntries.Where(w => w.State == FilteredQueryTimeEntryItemContractBase.BanState).Select(s => s.Id));
        processorResult.BannedTimeEntriesCount = bansAssignInvoiceResult.AssignedTimeEntriesCount;

        return processorResult;
    }
}
