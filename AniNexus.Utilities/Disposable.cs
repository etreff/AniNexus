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
        private readonly Action?[] _actions;
        private readonly Func<Task>?[] _funcs;
        private readonly bool _suppressErrors;

        public DelegateDisposable(Action? action, bool suppressErrors)
            : this(new[] { action }, suppressErrors)
        {
        }

        public DelegateDisposable(Action?[]? actions, bool suppressErrors)
        {
            _actions = actions ?? Array.Empty<Action>();
            _funcs = Array.Empty<Func<Task>>();
            _suppressErrors = suppressErrors;
        }

        public DelegateDisposable(Func<Task>? action, bool suppressErrors)
            : this(new[] { action }, suppressErrors)
        {
        }

        public DelegateDisposable(Func<Task>?[]? actions, bool suppressErrors)
        {
            _actions = Array.Empty<Action>();
            _funcs = actions ?? Array.Empty<Func<Task>>();
            _suppressErrors = suppressErrors;
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
            foreach (var action in _actions)
            {
                if (action is null)
                {
                    continue;
                }

                try
                {
                    action.Invoke();
                }
                catch when (_suppressErrors)
                {
                    // Suppress
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var action in _funcs)
            {
                if (action is null)
                {
                    continue;
                }

                try
                {
                    await action.Invoke();
                }
                catch when (_suppressErrors)
                {
                    // Suppress
                }
            }
        }
    }

    private sealed class CompositeDisposable : IDisposable, IAsyncDisposable
    {
        private readonly IDisposable?[] _disposables;
        private readonly IAsyncDisposable?[] _asyncDisposables;

        public CompositeDisposable(params IDisposable?[]? disposables)
        {
            _disposables = disposables ?? Array.Empty<IDisposable?>();
            _asyncDisposables = Array.Empty<IAsyncDisposable?>();
        }

        public CompositeDisposable(params IAsyncDisposable?[]? disposables)
        {
            _disposables = Array.Empty<IDisposable?>();
            _asyncDisposables = disposables ?? Array.Empty<IAsyncDisposable?>();
        }

        public CompositeDisposable(IDisposable?[]? disposables, IAsyncDisposable?[]? asyncDisposables)
        {
            _disposables = disposables ?? Array.Empty<IDisposable?>();
            _asyncDisposables = asyncDisposables ?? Array.Empty<IAsyncDisposable?>();
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
            foreach (var disposable in _disposables)
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
            foreach (var disposable in _asyncDisposables)
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
