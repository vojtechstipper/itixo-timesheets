using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class InvoiceRepository : AppRepositoryBase<Invoice, int>, IInvoiceRepository
{
    public InvoiceRepository(IDbContext dbContext) : base(dbContext) { }

    public Task<Invoice> GetByNumberAsync(string number)
    {
        return dbContext.Invoices.FirstOrDefaultAsync(invoice => invoice.Number == number);
    }

    public async Task<int> AddAsync(Invoice invoice)
    {
        EntityEntry<Invoice> entry = await dbContext.Invoices.AddAsync(invoice);
        await dbContext.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task<Invoice> GetOrCreateInvoice(string invoiceNumber)
    {
        Invoice invoice = await GetByNumberAsync(invoiceNumber);

        if (invoice == null)
        {
            invoice = Invoice.Create(invoiceNumber);
            await AddAsync(invoice);
        }

        return invoice;
    }
}
