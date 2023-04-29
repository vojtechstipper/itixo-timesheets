using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain.Synchronization;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IInvalidTimeEntriesReportRepository : IEntityRepository<InvalidTimeEntriesReport, int>
{
    public Task<InvalidTimeEntriesReport> GetByEmailAsync(string email, CancellationToken token = default);
}
