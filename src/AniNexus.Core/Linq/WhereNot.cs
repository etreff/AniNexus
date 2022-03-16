using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Linq;

public static partial class Linq
{
    /// <summary>
    /// Returns the collection where elements that do not match the predicate are removed.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="predicate"></param>
    public static IEnumerable<T?> WhereNot<T>(this IEnumerable<T?> collection, Func<T?, bool> predicate)
    {
        Guard.IsNotNull(collection, nameof(collection));
        Guard.IsNotNull(predicate, nameof(predicate));

        return _(collection, predicate);

        static IEnumerable<T?> _(IEnumerable<T?> collection, Func<T?, bool> predicate)
        {
            foreach (var element in collection)
            {
                if (!predicate(element))
                {
                    yield return element;
                }
            }
        }
    }
}
