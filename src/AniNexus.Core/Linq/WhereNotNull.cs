using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Linq;

public static partial class Linq
{
    /// <summary>
    /// Returns the collection where elements that are not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    /// <param name="collection">The collection.</param>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> collection)
    {
        Guard.IsNotNull(collection, nameof(collection));

        return _(collection);

        static IEnumerable<T> _(IEnumerable<T?> collection)
        {
            foreach (var element in collection)
            {
                if (element is not null)
                {
                    yield return element;
                }
            }
        }
    }
}
