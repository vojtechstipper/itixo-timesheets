using Itixo.Timesheets.Contracts.Invoices;

namespace Itixo.Timesheets.Admin.Client.Models.InvoiceModal;

public class InvoiceModalContext
{
    public string Number { get; set; }
    public bool IsDisplayed { get; set; }
    public string HeaderText { get; set; }
    public bool IncludeApproved { get; set; }
    public bool IncludeDrafts { get; set; }
    public bool IncludeBans { get; set; }
    public int TimeTrackerAccountId { get; set; }
    public int ApprovedTimeEntriesCount { get; set; }
    public int BannedTimeEntriesCount { get; set; }
    public int DraftedTimeEntriesCount { get; set; }
    public bool IsIncludeOfferStateActive { get; set; }
    public bool IsSetInvoiceStateActive { get; set; }
    public bool IsShowResultsStateActive { get; set; }

    public void IncludeOffer()
    {
        IsSetInvoiceStateActive = false;
        IsIncludeOfferStateActive = true;
        HeaderText = "Které záznamy chcete zahrnout";
    }

    public void Clear()
    {
        IsDisplayed = false;
        IsIncludeOfferStateActive = false;
        IsSetInvoiceStateActive = false;
        IsShowResultsStateActive = false;
        Number = string.Empty;
        IncludeBans = false;
        IncludeApproved = false;
        IncludeDrafts = false;
        TimeTrackerAccountId = 0;
        HeaderText = string.Empty;
    }

    public void SetInvoice(int timeTrackerAccountId)
    {
        IsDisplayed = true;
        HeaderText = "Zadejte číslo faktury";
        IsSetInvoiceStateActive = true;
        IsIncludeOfferStateActive = false;
        TimeTrackerAccountId = timeTrackerAccountId;
    }

    public void ShowResults(InvoiceAssignmentResult invoiceAssignmentResult)
    {
        IsSetInvoiceStateActive = false;
        IsIncludeOfferStateActive = false;
        HeaderText = "Výsledky přiřazení faktury";
        IsShowResultsStateActive = true;

        ApprovedTimeEntriesCount = invoiceAssignmentResult.ApprovedTimeEntriesCount;
        BannedTimeEntriesCount = invoiceAssignmentResult.BannedTimeEntriesCount;
        DraftedTimeEntriesCount = invoiceAssignmentResult.DraftedTimeEntriesCount;
    }
}
