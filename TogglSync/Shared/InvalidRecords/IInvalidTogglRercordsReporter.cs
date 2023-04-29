using Itixo.Timesheets.Shared.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TogglSyncShared.InvalidRecords;

public interface IInvalidTogglRercordsReporter : IService
{
    Task ReportInvalidRecords(List<ReportersInvalidRecord> invalidRecords, string email);
}
