using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AniNexus.Web;

/// <summary>
/// Provides mechanisms to execute <see cref="EventCallback"/> and
/// <see cref="EventCallback{TValue}"/> callbacks without triggering
/// a re-render.
/// </summary>
public sealed class NoRefreshEventCallback : IHandleEvent
{
    private static readonly NoRefreshEventCallback _instance = new();

    /// <summary>
    /// Creates a <see cref="NoRefreshEventCallback"/> using the specified action.
    /// </summary>
    /// <param name="callback">The callback to execute.</param>
    /// <returns>An event callback that can be passed to a razor component.</returns>
    public static EventCallback Create(Action callback)
    {
        return new EventCallback(_instance, callback);
    }

    /// <summary>
    /// Creates a <see cref="NoRefreshEventCallback"/> using the specified action.
    /// </summary>
    /// <param name="callback">The callback to execute.</param>
    /// <returns>An event callback that can be passed to a razor component.</returns>
    public static EventCallback<T> Create<T>(Action<T> callback)
    {
        return new EventCallback<T>(_instance, callback);
    }

    /// <summary>
    /// Creates a <see cref="NoRefreshEventCallback"/> using the specified action.
    /// </summary>
    /// <param name="callback">The callback to execute.</param>
    /// <returns>An event callback that can be passed to a razor component.</returns>
    public static EventCallback Create(Func<Task> callback)
    {
        return new EventCallback(_instance, callback);
    }

    /// <summary>
    /// Creates a <see cref="NoRefreshEventCallback"/> using the specified action.
    /// </summary>
    /// <param name="callback">The callback to execute.</param>
    /// <returns>An event callback that can be passed to a razor component.</returns>
    public static EventCallback<T> Create<T>(Func<T, Task> callback)
    {
        return new EventCallback<T>(_instance, callback);
    }

    private NoRefreshEventCallback()
    {
    }

    /// <inheritdoc />
    public Task HandleEventAsync(EventCallbackWorkItem callback, object? arg)
    {
        return callback.InvokeAsync(arg);
    }
}
