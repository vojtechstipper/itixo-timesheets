using System.Threading.Tasks;
using Itixo.Timesheets.Domain;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IInvoiceRepository : IEntityRepository<Invoice, int>
{
    Task<Invoice> GetOrCreateInvoice(string number);
}
