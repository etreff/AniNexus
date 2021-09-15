using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;

namespace AniNexus.Domain.Triggers;

public abstract class BeforeSaveTriggerBase<T> : IBeforeSaveTrigger<T>
    where T : class
{
    protected ApplicationDbContext DbContext { get; }

    protected BeforeSaveTriggerBase(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static bool IsEntityDeleted(EntityEntry entry, bool detachedIsDeleted = true)
    {
        return entry.State == EntityState.Deleted || (detachedIsDeleted && entry.State == EntityState.Detached);
    }

    public abstract Task BeforeSave(ITriggerContext<T> context, CancellationToken cancellationToken);
}

public abstract class AfterSaveTriggerBase<T> : IAfterSaveTrigger<T>
    where T : class
{
    protected ApplicationDbContext DbContext { get; }

    protected AfterSaveTriggerBase(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public abstract Task AfterSave(ITriggerContext<T> context, CancellationToken cancellationToken);
}
