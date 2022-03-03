using System.Collections.Concurrent;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Collections.Concurrent;

/// <summary>
/// Represents a thread-safe cache.
/// </summary>
/// <typeparam name="TKey">The key type of the cache.</typeparam>
/// <typeparam name="TValue">The value type of the cache.</typeparam>
public class ThreadSafeCache<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// The keys of the elements in this cache.
    /// </summary>
    public ICollection<TKey> Keys => _cache.Keys;

    /// <summary>
    /// The values of the elements in this cache.
    /// </summary>
    public ICollection<TValue> Values => _cache.Values;

    private readonly ConcurrentDictionary<TKey, TValue> _cache;
    private readonly Func<TKey, TValue> _valueFactory;

    /// <summary>
    /// Creates a new <see cref="ThreadSafeCache{TKey, TValue}"/> instance.
    /// </summary>
    /// <param name="valueFactory">The factory for creating a value if it doesn't exist in the cache.</param>
    public ThreadSafeCache(Func<TKey, TValue> valueFactory)
    {
        Guard.IsNotNull(valueFactory, nameof(valueFactory));

        _cache = new ConcurrentDictionary<TKey, TValue>();
        _valueFactory = valueFactory;
    }

    /// <summary>
    /// Creates a new <see cref="ThreadSafeCache{TKey, TValue}"/> instance.
    /// </summary>
    /// <param name="valueFactory">The factory for creating a value if it doesn't exist in the cache.</param>
    /// <param name="keyComparer">The key comparer.</param>
    public ThreadSafeCache(Func<TKey, TValue> valueFactory, IEqualityComparer<TKey>? keyComparer)
    {
        Guard.IsNotNull(valueFactory, nameof(valueFactory));

        _cache = new ConcurrentDictionary<TKey, TValue>(keyComparer);
        _valueFactory = valueFactory;
    }

    /// <summary>
    /// Gets the value from the cache under the key defined in <paramref name="key"/>.
    /// If the key does not exist in the cache, it is created in a thread-safe manner
    /// using the value factory specified in the constructor of this class.
    /// </summary>
    /// <param name="key">The key to get the value of.</param>
    public TValue Get([DisallowNull] TKey key)
    {
        return _cache.GetOrAdd(key, _valueFactory);
    }

    /// <summary>
    /// Attempts to get the value associated with the specified key from the <see cref="ThreadSafeCache{TKey, TValue}"/>.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">
    /// When this method returns, contains the object from the <see cref="ThreadSafeCache{TKey, TValue}"/>
    /// that has the specified key, or the default value of the type if the operation
    /// failed.</param>
    public bool TryGetValue([DisallowNull] TKey key, [MaybeNull] out TValue value)
    {
        return _cache.TryGetValue(key, out value);
    }
}
