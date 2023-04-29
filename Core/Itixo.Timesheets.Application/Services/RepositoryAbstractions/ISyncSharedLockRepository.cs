using System;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Configurations;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface ISyncSharedLockRepository
{
    Task LockSynchornization(DateTimeOffset dateTimeNow);
    Task UnlockSynchornization(DateTimeOffset dateTimeNow);
    Task<SyncSharedLockContract> GetCurrent();
}
