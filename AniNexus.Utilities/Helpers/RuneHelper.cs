using System.Buffers;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using AniNexus.Collections;

namespace AniNexus.Helpers;

/// <summary>
/// A utility for working with <see cref="Rune"/> instances.
/// </summary>
public static class RuneHelper
{
    /// <summary>
    /// Returns the similarity percentage of two collections of <see cref="Rune"/>.
    /// </summary>
    /// <param name="sourceRunes">The first text instance.</param>
    /// <param name="otherRunes">The second text instance.</param>
    /// <param name="comparison">How to compare two chars.</param>
    /// <returns>A number between 0 and 1 that represents a percentage similarity between the two strings.</returns>
    /// <remarks>
    /// If both arguments are empty, 1 will be returned.
    /// </remarks>
    public static double SimilarityTo(List<Rune> sourceRunes, List<Rune> otherRunes, StringComparison comparison)
    {
        int steps = DamerauLevenshteinDistance(sourceRunes, otherRunes, comparison);
        return 1 - steps / (double)Math.Max(sourceRunes.Count, otherRunes.Count);
    }

    /// <summary>
    /// Returns the number of steps needed to change the contents of <paramref name="sourceRunes"/> into <paramref name="otherRunes"/>.
    /// </summary>
    /// <param name="sourceRunes">The first text instance.</param>
    /// <param name="otherRunes">The second text instance.</param>
    /// <param name="comparison">How to compare two chars.</param>
    /// <returns>The number of steps needed to change <paramref name="sourceRunes"/> into <paramref name="otherRunes"/>.</returns>
    /// <exception cref="NotImplementedException">The specified comparison is not implemented between two Runes with different ordinal values.</exception>
    private static int DamerauLevenshteinDistance(List<Rune> sourceRunes, List<Rune> otherRunes, StringComparison comparison = StringComparison.Ordinal)
    {
        int spanLength = sourceRunes.Count;
        int otherLength = otherRunes.Count;

        if (spanLength == 0)
        {
            return otherLength;
        }

        if (otherLength == 0)
        {
            return spanLength;
        }

        // Finding distances here utilizes a sort of grid. We use that as a lookup for our real word count.
        int[] buffer = ArrayPool<int>.Shared.Rent((spanLength + 1) * (otherLength + 1));
        try
        {
            var costs = new Array2D<int>(buffer, spanLength + 1, otherLength + 1);

            for (int x = 0; x <= spanLength; costs[x, 0] = x++) { }
            for (int y = 0; y <= otherLength; costs[0, y] = y++) { }

            for (int x = 1; x <= spanLength; ++x)
            {
                for (int y = 1; y <= otherLength; ++y)
                {
                    var sourceRune = sourceRunes[x - 1];
                    var otherRune = otherRunes[y - 1];

                    int cost = RunesEqual(sourceRune, otherRune, comparison) ? 0 : 1;
                    int insertion = costs[x, y - 1] + 1;
                    int deletion = costs[x - 1, y] + 1;
                    int substitution = costs[x - 1, y - 1] + cost;

                    int distance = Math.Min(Math.Min(insertion, deletion), substitution);

                    if (x > 1 && y > 1 &&
                        RunesEqual(sourceRunes[x - 1], otherRunes[y - 2], comparison) &&
                        RunesEqual(sourceRunes[x - 2], otherRunes[y - 1], comparison))
                    {
                        distance = Math.Min(distance, costs[x - 2, y - 2] + cost);
                    }

                    costs[x, y] = distance;
                }
            }

            return costs[spanLength, otherLength];
        }
        finally
        {
            ArrayPool<int>.Shared.Return(buffer);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool RunesEqual(in Rune a, in Rune b, StringComparison comparison)
    {
        // Equal ordinal values are similar in every comparison.
        if (a == b)
        {
            return true;
        }

        // If the ordinal values do not match up case-sensitively, immediately mark it as a mismatch
        // for case-sensitive comparisons.
        if (comparison == StringComparison.Ordinal ||
            comparison == StringComparison.CurrentCulture ||
            comparison == StringComparison.InvariantCulture)
        {
            return false;
        }

        var culture = comparison switch
        {
            StringComparison.CurrentCultureIgnoreCase => CultureInfo.CurrentCulture,
            StringComparison.InvariantCultureIgnoreCase => CultureInfo.InvariantCulture,
            _ => CultureInfo.InvariantCulture
        };

        if (Rune.IsLower(a))
        {
            if (Rune.ToUpper(a, culture) == b)
            {
                return true;
            }

            // If the culture does something weird to upper-case, try
            // seeing whether converting to lowercase and comparing works.
            return Rune.ToLower(b, culture) == a;
        }
        else if (Rune.IsLower(b))
        {
            if (Rune.ToUpper(b, culture) == a)
            {
                return true;
            }

            // If the culture does something weird to upper-case, try
            // seeing whether converting to lowercase and comparing works.
            return Rune.ToLower(b, culture) == a;
        }

        return false;
    }
}
