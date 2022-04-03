using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AniNexus.Threading;

/// <summary>
/// A global <see cref="Mutex"/> class that applies
/// cross-application.
/// </summary>
[DebuggerDisplay("{" + nameof(Name) + "}")]
public sealed class GlobalMutex : IDisposable
{
    /// <summary>
    /// The name of the mutex.
    /// </summary>
    public string Name { get; }

    private readonly Mutex _mutex;
    private readonly object _lock = new();

    /// <summary>
    /// Creates a <see cref="GlobalMutex"/> using the entry assembly's <see cref="GuidAttribute"/>
    /// for the name. If the assembly does not contain this attribute, an <see cref="InvalidOperationException"/>
    /// will be thrown.
    /// </summary>
    /// <exception cref="WaitHandleCannotBeOpenedException">The named mutex cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
    /// <exception cref="IOException">A Win32 error occurred.</exception>
    /// <exception cref="UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl"></see>.</exception>
    public GlobalMutex()
        : this(GetAppGuid())
    {
    }

    /// <summary>
    /// Creates a <see cref="GlobalMutex"/>.
    /// </summary>
    /// <param name="mutexName">The name of the mutex.</param>
    /// <exception cref="UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl"></see>.</exception>
    /// <exception cref="IOException">A Win32 error occurred.</exception>
    /// <exception cref="WaitHandleCannotBeOpenedException">The named mutex cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
    /// <exception cref="ArgumentException"><paramref name="mutexName"/> is null or empty.</exception>
    public GlobalMutex(string mutexName)
    {
        Guard.IsNotNullOrWhiteSpace(mutexName, nameof(mutexName));

        Name = mutexName.Replace("\\", "");
        _mutex = new Mutex(false, $"Global\\{Name}");
    }

    /// <inheritdoc />
    ~GlobalMutex()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        lock (_lock)
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    private void Dispose(bool disposing)
    {
        lock (_lock)
        {
            if (disposing)
            {
                _mutex?.Dispose();
            }
        }
    }

    /// <summary>
    /// Attempts to acquire the mutex.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public GlobalMutexLockInfo TryAcquire(CancellationToken cancellationToken = default)
        => TryAcquire(Timeout.InfiniteTimeSpan, cancellationToken);

    /// <summary>
    /// Attempts to acquire the mutex.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public GlobalMutexLockInfo TryAcquire(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        try
        {
            bool hasAcquiredMutex = _mutex.WaitOne(timeout, cancellationToken);
            return new GlobalMutexLockInfo(hasAcquiredMutex, hasAcquiredMutex ? Disposable.Create(new Action(_mutex.ReleaseMutex)) : Disposable.Empty);
        }
        catch (TaskCanceledException)
        {
            return new GlobalMutexLockInfo(false, Disposable.Empty);
        }
        catch (AbandonedMutexException)
        {
            // Abandoned mutexes are still acquired.
            return new GlobalMutexLockInfo(true, Disposable.Create(new Action(_mutex.ReleaseMutex)));
        }
    }

    /// <summary>
    /// Attempts to acquire the mutex.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public Task<GlobalMutexLockInfo> TryAcquireAsync(CancellationToken cancellationToken)
        => TryAcquireAsync(Timeout.InfiniteTimeSpan, cancellationToken);

    /// <summary>
    /// Attempts to acquire the mutex.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task<GlobalMutexLockInfo> TryAcquireAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        bool acquiredMutex = false;
        var mre = new ManualResetEventSlim();
        var mre2 = new ManualResetEventSlim();

        // Spawn thread to handle mutex creation and disposal.
        // This needs to happen on its own dedicated thread. We cannot
        // use async/await for this due to thread switching even with
        // ConfigureAwait(true).
        // This also means we are non-reentrant.
        var mutexThread = new Thread(() =>
        {
            try
            {
                acquiredMutex = _mutex.WaitOne(timeout, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                acquiredMutex = false;
            }
            catch (AbandonedMutexException)
            {
                    // Abandoned mutexes are still acquired.
                    acquiredMutex = true;
            }
            catch
            {
                acquiredMutex = false;
            }

                // Inform the outer method that we have determined
                // whether we have acquired the mutex.
                mre2.Set();

            if (acquiredMutex)
            {
                    // If we have acquired the mutex, wait until the caller
                    // disposes the lock info.
                    mre.Wait();
                try
                {
                    _mutex.ReleaseMutex();
                }
                catch
                {
                        // Suppress
                    }
            }
        })
        {
            // Kills the thread if the application exists. This should cause
            // an AbandonedMutexException in other instances, allowing them
            // to acquire it.
            IsBackground = true
        };

        mutexThread.Start();

        // Wait until mutex acquisition either succeeds or fails.
        await mre2.WaitHandle.ConfigureAwait(false);
        mre2.Dispose();

        if (!acquiredMutex)
        {
            // If we did not acquire the mutex we have no more need
            // for this MRE. Dispose of it.
            mre.Dispose();
        }

        // Return a new lock instance that either no-ops if we did not acquire the mutex or 
        // triggers the MRE on our mutex thread and release the mutex if we did acquire it.
        return new GlobalMutexLockInfo(acquiredMutex, acquiredMutex ? new GlobalMutexLockFree(mre) : Disposable.Empty);
    }

    private static string GetAppGuid()
    {
        var guidAttr = Assembly.GetEntryAssembly()?.GetCustomAttribute<GuidAttribute>();
        return guidAttr is not null
            ? Guid.Parse(guidAttr.Value).ToString("B")
            : throw new InvalidOperationException($"Assembly is missing a {nameof(GuidAttribute)}.");
    }

    private sealed class GlobalMutexLockFree : IDisposable
    {
        private readonly ManualResetEventSlim _resetEvent;

        public GlobalMutexLockFree(ManualResetEventSlim resetEvent)
        {
            _resetEvent = resetEvent;
        }

        ~GlobalMutexLockFree()
        {
            Dispose();
        }

        public void Dispose()
        {
            _resetEvent.Set();
            _resetEvent.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
