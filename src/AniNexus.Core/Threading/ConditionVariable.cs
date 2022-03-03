namespace AniNexus.Threading;

/// <summary>
/// A variable that will block until it is set.
/// </summary>
/// <typeparam name="T">The variable type.</typeparam>
/// <remarks>
/// This behaves like a synchronous <see cref="TaskCompletionSource{TResult}"/>,
/// or a <see cref="ManualResetEvent"/> with a return type.
/// </remarks>
public sealed class ConditionVariable<T> : IDisposable
{
    /// <summary>
    /// The synchronization object.
    /// </summary>
    public object SyncRoot { get; } = new object();

    private readonly ManualResetEvent _resetEvent = new(false);

    [AllowNull, MaybeNull]
    private T _value;

    private bool _isSet;

    /// <summary>
    /// Creates a new instance of <see cref="ConditionVariable{T}"/> that is
    /// not set.
    /// </summary>
    public ConditionVariable()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="ConditionVariable{T}"/> that is
    /// set with the provided value.
    /// </summary>
    /// <param name="initialValue">The initial value.</param>
    public ConditionVariable(T? initialValue)
    {
        Set(initialValue);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="ConditionVariable{T}"/> class.
    /// </summary>
    public void Dispose()
    {
        _resetEvent.Dispose();
    }

    /// <summary>
    /// Waits for the variable to be set and returns the result.
    /// </summary>
    [return: MaybeNull]
    public T Wait()
    {
        _resetEvent.WaitOne();
        return _value;
    }

    /// <summary>
    /// Waits for the variable to be set and returns the result.
    /// </summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait before timing out.</param>
    /// <exception cref="TimeoutException">The operation timed out.</exception>
    [return: MaybeNull]
    public T Wait(int millisecondsTimeout)
    {
        return _resetEvent.WaitOne(millisecondsTimeout)
            ? _value
            : throw new TimeoutException("Unable to wait for variable. The operation timed out.");
    }

    /// <summary>
    /// Waits for the variable to be set and returns the result.
    /// </summary>
    /// <param name="timeout">The duration to wait before timing out.</param>
    /// <exception cref="TimeoutException">The operation timed out.</exception>
    [return: MaybeNull]
    public T Wait(TimeSpan timeout)
    {
        return _resetEvent.WaitOne(timeout)
            ? _value
            : throw new TimeoutException("Unable to wait for variable. The operation timed out.");
    }

    /// <summary>
    /// Waits for the variable to be set and returns the result.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="TimeoutException">The operation timed out.</exception>
    public async Task<T?> WaitAsync(CancellationToken cancellationToken = default)
    {
        await _resetEvent.WaitOneAsync(Timeout.Infinite, cancellationToken).ConfigureAwait(false);
        return _value!;
    }

    /// <summary>
    /// Waits for the variable to be set and returns the result.
    /// </summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait before timing out.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="TimeoutException">The operation timed out.</exception>
    public async Task<T?> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken = default)
    {
        return await _resetEvent.WaitOneAsync(millisecondsTimeout, cancellationToken).ConfigureAwait(false)
            ? _value
            : throw new TimeoutException("Unable to wait for variable. The operation timed out.");
    }

    /// <summary>
    /// Waits for the variable to be set and returns the result.
    /// </summary>
    /// <param name="timeout">The duration to wait before timing out.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="TimeoutException">The operation timed out.</exception>
    public async Task<T?> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        return await _resetEvent.WaitOneAsync(timeout, cancellationToken).ConfigureAwait(false)
            ? _value
            : throw new TimeoutException("Unable to wait for variable. The operation timed out.");
    }

    /// <summary>
    /// Sets the value of this variable.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidOperationException">The variable has already been set.</exception>
    public void Set(T? value)
    {
        lock (SyncRoot)
        {
            if (_isSet)
            {
                throw new InvalidOperationException("The variable has already been set.");
            }

            _value = value;
            _resetEvent.Set();
            _isSet = true;
        }
    }

    /// <summary>
    /// Unsets the value of this variable.
    /// </summary>
    public void Unset()
    {
        lock (SyncRoot)
        {
            _value = default;
            _resetEvent.Reset();
            _isSet = false;
        }
    }
}
