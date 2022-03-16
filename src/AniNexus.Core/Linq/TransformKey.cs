using System.Collections;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Linq;

public static partial class Linq
{
    /// <summary>
    /// Transforms a grouping with one key to a grouping with another key.
    /// </summary>
    /// <typeparam name="TOldKey">The type of the old key.</typeparam>
    /// <typeparam name="TValue">The type of the grouping.</typeparam>
    /// <typeparam name="TNewKey">The type of the new key.</typeparam>
    /// <param name="grouping">The grouping.</param>
    /// <param name="reducer">The method that gets the new key to group by.</param>
    public static IEnumerable<IGrouping<TNewKey, TValue>> TransformKey<TOldKey, TValue, TNewKey>(this IEnumerable<IGrouping<TOldKey, TValue>> grouping, Func<TOldKey, TNewKey> reducer)
    {
        Guard.IsNotNull(grouping, nameof(grouping));
        Guard.IsNotNull(reducer, nameof(reducer));

        return _(grouping, reducer);

        static IEnumerable<IGrouping<TNewKey, TValue>> _(IEnumerable<IGrouping<TOldKey, TValue>> grouping, Func<TOldKey, TNewKey> reducer)
        {
            foreach (var group in grouping)
            {
                yield return new Grouping<TNewKey, TValue>(reducer(group.Key), group);
            }
        }
    }

    private sealed class Grouping<TKey, TValue> : IGrouping<TKey, TValue>
    {
        public TKey Key { get; }
        private readonly IEnumerable<TValue> _value;

        public Grouping(TKey key, IEnumerable<TValue> value)
        {
            Key = key;
            _value = value;
        }

        public IEnumerator<TValue> GetEnumerator() => _value.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
