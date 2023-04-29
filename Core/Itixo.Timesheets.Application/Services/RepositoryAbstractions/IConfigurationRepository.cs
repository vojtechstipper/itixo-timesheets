using Itixo.Timesheets.Domain.Application;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IConfigurationRepository : IEntityRepository<Configuration, int>
{
    Task AddOrUpdateSyncHourlyRepetitionAsync(int syncHourlyRepetition);
    Task<bool> IsUniqueAsync(string key);
    Task<Configuration> GetByKeyAsync(string key, CancellationToken token = default);
    Task<bool> AnyAsync(int id, CancellationToken token = default);
}
