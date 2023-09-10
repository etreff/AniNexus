namespace AniNexus.Threading;

/// <summary>
/// A <see langword="lock"/> alternative that works in async contexts.
/// This implementation is lighter than <see cref="AsyncLock"/> but
/// does not allow same thread re-entry.
/// </summary>
public sealed class AsyncLockSlim : IDisposable
{
    private readonly SemaphoreSlim _lock;

    /// <summary>
    /// Creates a new <see cref="AsyncLockSlim"/> instance.
    /// </summary>
    public AsyncLockSlim()
        : this(1, 1)
    {
    }

    /// <summary>
    /// Creates a new <see cref="AsyncLockSlim"/> instance.
    /// </summary>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="maxCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
    public AsyncLockSlim(int initialCount, int maxCount)
    {
        _lock = new SemaphoreSlim(initialCount, maxCount);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _lock.Dispose();
    }

    /// <summary>
    /// Infinitely waits for the thread to enter the lock.
    /// </summary>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public IDisposable Wait()
        => Wait(Timeout.Infinite);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The number of milliseconds to wait for the lock.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public IDisposable Wait(int timeout)
        => Wait(TimeSpan.FromMilliseconds(timeout));

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public IDisposable Wait(TimeSpan timeout)
        => Wait(timeout, CancellationToken.None);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public IDisposable Wait(CancellationToken cancellationToken)
        => Wait(Timeout.InfiniteTimeSpan, cancellationToken);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The number of milliseconds to wait for the lock.</param>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public IDisposable Wait(int timeout, CancellationToken cancellationToken)
        => Wait(TimeSpan.FromMilliseconds(timeout), cancellationToken);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public IDisposable Wait(TimeSpan timeout, CancellationToken cancellationToken)
    {
        return _lock.Wait(timeout, cancellationToken)
            ? new AsyncLockRelease(this)
            : throw new InvalidOperationException("Cannot acquire the API lock.");
    }

    /// <summary>
    /// Infinitely waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public Task<IDisposable> WaitAsync(int timeout)
        => WaitAsync(TimeSpan.FromMilliseconds(timeout));

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public Task<IDisposable> WaitAsync(TimeSpan timeout)
        => WaitAsync(timeout, CancellationToken.None);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public Task<IDisposable> WaitAsync(CancellationToken cancellationToken)
        => WaitAsync(Timeout.InfiniteTimeSpan, cancellationToken);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The number of milliseconds to wait for the lock.</param>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public Task<IDisposable> WaitAsync(int timeout, CancellationToken cancellationToken)
        => WaitAsync(TimeSpan.FromMilliseconds(timeout), cancellationToken);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that frees the lock when disposed.</returns>
    /// <exception cref="InvalidOperationException">The lock could not be acquired.</exception>
    public async Task<IDisposable> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        return await _lock.WaitAsync(timeout, cancellationToken).ConfigureAwait(false)
            ? new AsyncLockRelease(this)
            : throw new InvalidOperationException("Cannot acquire lock.");
    }

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The number of milliseconds to wait for the lock.</param>
    /// <returns>An object that contains whether the lock was successfully acquired and an object that frees the lock when disposed.</returns>
    public Task<(bool AcquiredLock, IDisposable LockFree)> TryWaitAsync(int timeout)
        => TryWaitAsync(TimeSpan.FromMilliseconds(timeout));

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <returns>An object that contains whether the lock was successfully acquired and an object that frees the lock when disposed.</returns>
    public Task<(bool AcquiredLock, IDisposable LockFree)> TryWaitAsync(TimeSpan timeout)
        => TryWaitAsync(timeout, CancellationToken.None);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that contains whether the lock was successfully acquired and an object that frees the lock when disposed.</returns>
    public Task<(bool AcquiredLock, IDisposable LockFree)> TryWaitAsync(CancellationToken cancellationToken)
        => TryWaitAsync(Timeout.InfiniteTimeSpan, cancellationToken);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The number of milliseconds to wait for the lock.</param>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that contains whether the lock was successfully acquired and an object that frees the lock when disposed.</returns>
    public Task<(bool AcquiredLock, IDisposable LockFree)> TryWaitAsync(int timeout, CancellationToken cancellationToken)
        => TryWaitAsync(TimeSpan.FromMilliseconds(timeout), cancellationToken);

    /// <summary>
    /// Waits for the thread to enter the lock.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the lock.</param>
    /// <param name="cancellationToken">A token that will cancel waiting for the lock when canceled.</param>
    /// <returns>An object that contains whether the lock was successfully acquired and an object that frees the lock when disposed.</returns>
    public async Task<(bool AcquiredLock, IDisposable LockFree)> TryWaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        return await _lock.AvailableWaitHandle.WaitOneAsync(timeout, cancellationToken).ConfigureAwait(false)
            ? (true, new AsyncLockRelease(this))
            : (false, Disposable.Empty);
    }

    private sealed class AsyncLockRelease : IDisposable
    {
        private AsyncLockSlim Owner { get; }
        private bool IsDisposed { get; set; }
        private readonly object _lock = new();

        public AsyncLockRelease(AsyncLockSlim owner)
        {
            Owner = owner;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                lock (_lock)
                {
                    if (!IsDisposed)
                    {
                        try
                        {
                            Owner._lock.Release();
                        }
                        catch (ObjectDisposedException)
                        {
                            // Suppress - the underlying lock may have already
                            // been disposed, in which case we can't do anything here.
                        }
                        IsDisposed = true;
                    }
                }
            }
        }
    }
}
