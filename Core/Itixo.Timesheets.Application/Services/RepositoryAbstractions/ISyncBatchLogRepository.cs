using System.Threading.Tasks;
using Itixo.Timesheets.Domain;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface ISyncBatchLogRepository
{
    Task AddAsync(SyncBatchLogRecord record);
}
