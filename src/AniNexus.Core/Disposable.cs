namespace AniNexus;

/// <summary>
/// A utility class for creating <see cref="IDisposable"/> instances.
/// </summary>
public static class Disposable
{
    /// <summary>
    /// An <see cref="IDisposable"/> that does nothing when disposed.
    /// </summary>
    /// <remarks>
    /// The same instance will be returned every time.
    /// </remarks>
    public static IDisposable Empty { get; } = new EmptyDisposable();

    /// <summary>
    /// An <see cref="IAsyncDisposable"/> that does nothing when disposed.
    /// </summary>
    /// <remarks>
    /// Unlike its <see cref="IDisposable"/> counterpart (<see cref="Empty"/>),
    /// a new instance is created with each invocation.
    /// </remarks>
    public static IAsyncDisposable EmptyAsync => new EmptyAsyncDisposable();

    /// <summary>
    /// Combines disposables into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="disposable">The <see cref="IDisposable"/> instance to combine.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates the provided <see cref="IDisposable"/> instance.</returns>
    public static IDisposable Combine(IDisposable? disposable)
        => new CompositeDisposable(disposable);

    /// <summary>
    /// Combines disposables into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="disposable">The <see cref="IDisposable"/> instance to combine.</param>
    /// <param name="asyncDisposable">The <see cref="IAsyncDisposable"/> instance to combine.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates the provided <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> instances.</returns>
    public static IDisposable Combine(IDisposable? disposable, IAsyncDisposable? asyncDisposable)
        => new CompositeDisposable(new[] { disposable }, new[] { asyncDisposable });

    /// <summary>
    /// Combines disposables into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="disposable">The <see cref="IDisposable"/> instance to combine.</param>
    /// <param name="asyncDisposable">The <see cref="IAsyncDisposable"/> instance to combine.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates the provided <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> instances.</returns>
    public static IAsyncDisposable CombineAsync(IDisposable? disposable, IAsyncDisposable? asyncDisposable)
        => new CompositeDisposable(new[] { disposable }, new[] { asyncDisposable });

    /// <summary>
    /// Combines disposables into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IDisposable"/> instances to combine.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable Combine(params IDisposable?[]? disposables)
        => new CompositeDisposable(disposables);

    /// <summary>
    /// Combines disposables into a single <see cref="IAsyncDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IAsyncDisposable"/> instance to combine.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IAsyncDisposable Combine(params IAsyncDisposable?[]? disposables)
        => new CompositeDisposable(disposables);

    /// <summary>
    /// Combines disposables into a single <see cref="IAsyncDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IDisposable"/> instances to combine.</param>
    /// <param name="asyncDisposables">The <see cref="IAsyncDisposable"/> instances to combine.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates the provided <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> instances.</returns>
    public static IDisposable Combine(IDisposable?[]? disposables, IAsyncDisposable?[]? asyncDisposables)
        => new CompositeDisposable(disposables, asyncDisposables);

    /// <summary>
    /// Combines disposables into a single <see cref="IAsyncDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IDisposable"/> instances to combine.</param>
    /// <param name="asyncDisposables">The <see cref="IAsyncDisposable"/> instances to combine.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates the provided <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> instances.</returns>
    public static IAsyncDisposable CombineAsync(IDisposable?[]? disposables, IAsyncDisposable?[]? asyncDisposables)
        => new CompositeDisposable(disposables, asyncDisposables);

    /// <summary>
    /// Combines disposables into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IDisposable"/> instances to combine.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable Combine(IEnumerable<IDisposable?>? disposables)
        => Combine(disposables?.ToArray());

    /// <summary>
    /// Combines disposables into a single <see cref="IAsyncDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IAsyncDisposable"/> instances to combine.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates all of the provided <see cref="IAsyncDisposable"/> instances.</returns>
    public static IAsyncDisposable Combine(IEnumerable<IAsyncDisposable?>? disposables)
        => Combine(disposables?.ToArray());

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="action">The action to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable CreateSafe(Action? action)
        => new DelegateDisposable(action, true);

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable CreateSafe(params Action?[]? actions)
        => new DelegateDisposable(actions, true);

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable CreateSafe(IEnumerable<Action?>? actions)
        => CreateSafe(actions?.ToArray());

    /// <summary>
    /// Combines actions into a single <see cref="IAsyncDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="action">The action to invoke when the <see cref="IAsyncDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates all of the provided <see cref="IAsyncDisposable"/> instances.</returns>
    public static IAsyncDisposable CreateSafe(Func<Task>? action)
        => new DelegateDisposable(action, true);

    /// <summary>
    /// Combines actions into a single <see cref="IAsyncDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IAsyncDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates all of the provided <see cref="IAsyncDisposable"/> instances.</returns>
    public static IAsyncDisposable CreateSafe(params Func<Task>?[]? actions)
        => new DelegateDisposable(actions, true);

    /// <summary>
    /// Combines actions into a single <see cref="IAsyncDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IAsyncDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IAsyncDisposable"/> that encapsulates all of the provided <see cref="IAsyncDisposable"/> instances.</returns>
    public static IAsyncDisposable CreateSafe(IEnumerable<Func<Task>?>? actions)
        => CreateSafe(actions?.ToArray());

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="action">The action to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable Create(Action? action)
        => new DelegateDisposable(action, false);

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable Create(params Action?[]? actions)
        => new DelegateDisposable(actions, false);

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IDisposable Create(IEnumerable<Action?>? actions)
        => Create(actions?.ToArray());

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="action">The action to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IAsyncDisposable Create(Func<Task>? action)
        => new DelegateDisposable(action, false);

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IAsyncDisposable Create(params Func<Task>?[]? actions)
        => new DelegateDisposable(actions, false);

    /// <summary>
    /// Combines actions into a single <see cref="IDisposable"/> instance. Exceptions thrown during
    /// action execution are suppressed.
    /// </summary>
    /// <param name="actions">The actions to invoke when the <see cref="IDisposable"/> is disposed.</param>
    /// <returns>An <see cref="IDisposable"/> that encapsulates all of the provided <see cref="IDisposable"/> instances.</returns>
    public static IAsyncDisposable Create(IEnumerable<Func<Task>?>? actions)
        => Create(actions?.ToArray());

    private sealed class DelegateDisposable : IDisposable, IAsyncDisposable
    {
        private readonly Action?[] Actions;
        private readonly Func<Task>?[] Funcs;
        private readonly bool SuppressErrors;

        public DelegateDisposable(Action? action, bool suppressErrors)
            : this(new[] { action }, suppressErrors)
        {

        }

        public DelegateDisposable(Action?[]? actions, bool suppressErrors)
        {
            Actions = actions ?? Array.Empty<Action>();
            Funcs = Array.Empty<Func<Task>>();
            SuppressErrors = suppressErrors;
        }

        public DelegateDisposable(Func<Task>? action, bool suppressErrors)
            : this(new[] { action }, suppressErrors)
        {

        }

        public DelegateDisposable(Func<Task>?[]? actions, bool suppressErrors)
        {
            Actions = Array.Empty<Action>();
            Funcs = actions ?? Array.Empty<Func<Task>>();
            SuppressErrors = suppressErrors;
        }

        void IDisposable.Dispose()
        {
            Dispose();
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            Dispose();
            await DisposeAsync();
        }

        public void Dispose()
        {
            foreach (var action in Actions)
            {
                if (action is null)
                {
                    continue;
                }

                try
                {
                    action.Invoke();
                }
                catch when (SuppressErrors)
                {
                    // Suppress
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var action in Funcs)
            {
                if (action is null)
                {
                    continue;
                }

                try
                {
                    await action.Invoke();
                }
                catch when (SuppressErrors)
                {
                    // Suppress
                }
            }
        }
    }

    private sealed class CompositeDisposable : IDisposable, IAsyncDisposable
    {
        private readonly IDisposable?[] Disposables;
        private readonly IAsyncDisposable?[] AsyncDisposables;

        public CompositeDisposable(params IDisposable?[]? disposables)
        {
            Disposables = disposables ?? Array.Empty<IDisposable?>();
            AsyncDisposables = Array.Empty<IAsyncDisposable?>();
        }

        public CompositeDisposable(params IAsyncDisposable?[]? disposables)
        {
            Disposables = Array.Empty<IDisposable?>();
            AsyncDisposables = disposables ?? Array.Empty<IAsyncDisposable?>();
        }

        public CompositeDisposable(IDisposable?[]? disposables, IAsyncDisposable?[]? asyncDisposables)
        {
            Disposables = disposables ?? Array.Empty<IDisposable?>();
            AsyncDisposables = asyncDisposables ?? Array.Empty<IAsyncDisposable?>();
        }

        void IDisposable.Dispose()
        {
            Dispose();
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            Dispose();
            await DisposeAsync();
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
            {
                if (disposable is null)
                {
                    continue;
                }

                try
                {
                    disposable.Dispose();
                }
                catch
                {
                    // Suppress
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var disposable in AsyncDisposables)
            {
                if (disposable is null)
                {
                    continue;
                }

                try
                {
                    await disposable.DisposeAsync();
                }
                catch
                {
                    // Suppress
                }
            }
        }
    }

    internal sealed class EmptyDisposable : IDisposable
    {
        /// <inheritdoc />
        public void Dispose()
        {
            // Method intentionally left empty.
        }
    }

    internal sealed class EmptyAsyncDisposable : IAsyncDisposable
    {
        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
