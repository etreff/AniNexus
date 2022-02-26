namespace AniNexus.Threading;

/// <summary>
/// Information about a <see cref="GlobalMutex"/> lock.
/// </summary>
public sealed class GlobalMutexLockInfo : IDisposable
{
    /// <summary>
    /// A <see cref="GlobalMutexLockInfo"/> instance that did not acquire a lock.
    /// </summary>
    public static GlobalMutexLockInfo Failed => new(false, Disposable.Empty);

    /// <summary>
    /// Whether the lock was successfully acquired.
    /// </summary>
    public bool AcquiredLock { get; }

    private readonly IDisposable _lockFree;
    private bool _disposed;

    internal GlobalMutexLockInfo(IDisposable lockFree)
    {
        _lockFree = lockFree;
    }

    internal GlobalMutexLockInfo(bool acquiredLock, IDisposable lockFree)
    {
        AcquiredLock = acquiredLock;
        _lockFree = lockFree;
    }

    /// <summary>
    /// Disposes the instance.
    /// </summary>
    ~GlobalMutexLockInfo()
    {
        DoDispose();
    }

    /// <summary>
    /// Releases the lock if <see cref="AcquiredLock"/> is <see langword="true"/>.
    /// </summary>
    public void Dispose()
    {
        DoDispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the mutex.
    /// </summary>
    public void ReleaseMutex()
    {
        Dispose();
    }

    private void DoDispose()
    {
        lock (_lockFree)
        {
            if (_disposed)
            {
                return;
            }

            _lockFree.Dispose();
            _disposed = true;
        }
    }
}

