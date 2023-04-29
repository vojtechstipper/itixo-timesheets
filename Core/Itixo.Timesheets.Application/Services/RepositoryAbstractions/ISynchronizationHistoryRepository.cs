using System;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface ISynchronizationHistoryRepository
{
    Task AddAsync(SyncLogRecord record);
    Task<SyncLogRecord> GetAsync(Guid id);
    Task UpdateAsync(SyncLogRecord syncLogRecord);
}
