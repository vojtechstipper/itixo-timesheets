using System;

namespace Itixo.Timesheets.Domain.Application;

public class SyncSharedLock
{
    public int Id { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }

    public static SyncSharedLock DEFAULT = new SyncSharedLock { End = DateTimeOffset.Now };

    private SyncSharedLock() { }

    public static SyncSharedLock CreateLock(DateTimeOffset dateTimeNow)
    {
        return new SyncSharedLock { Start = dateTimeNow };
    }

    public SyncSharedLock UnLock(DateTimeOffset dateTimeNow)
    {
        End = dateTimeNow;
        return this;
    }

    public bool IsLocked()
    {
        return End == null;
    }

}
