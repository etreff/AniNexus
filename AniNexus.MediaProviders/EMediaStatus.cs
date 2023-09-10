using System.Collections.Immutable;

namespace AniNexus.MediaProviders;

/// <summary>
/// The status of a piece of media.
/// </summary>
public enum EMediaStatus
{
    /// <summary>
    /// Has completed and is no longer being released
    /// </summary>
    Finished,

    /// <summary>
    /// Currently releasing
    /// </summary>
    Releasing,

    /// <summary>
    /// To be released at a later date
    /// </summary>
    NotYetReleased,

    /// <summary>
    /// Ended before the work could be finished
    /// </summary>
    Canceled,

    /// <summary>
    /// Version 2 only. Is currently paused from releasing and will resume at a later date
    /// </summary>
    Hiatus
}

/// <summary>
/// A helper class for working with <see cref="EMediaStatus"/>.
/// </summary>
public static class MediaStatusHelper
{
    private static readonly IImmutableDictionary<EMediaStatus, int> _sortOrder = ImmutableDictionary.CreateRange(new KeyValuePair<EMediaStatus, int>[]
    {
            new(EMediaStatus.Releasing, 0),
            new(EMediaStatus.NotYetReleased, 1),
            new(EMediaStatus.Finished, 2)
    });

    /// <summary>
    /// Returns a comparer that sorts <see cref="EMediaStatus"/> putting the more common types first.
    /// </summary>
    /// <remarks>
    /// This comparer is primarily used for sorting statuses to be displayed.
    /// </remarks>
    public static IComparer<EMediaStatus> Comparer { get; } = Comparer<EMediaStatus>.Create(static (a, b) =>
    {
        int aSort = _sortOrder.TryGetValue(a, out int aVal) ? aVal : int.MaxValue;
        int bSort = _sortOrder.TryGetValue(b, out int bVal) ? bVal : int.MaxValue;

        if (aSort == int.MaxValue)
        {
            if (bSort != int.MaxValue)
            {
                return 1;
            }

            return a.CompareTo(b);
        }

        if (bSort == int.MaxValue)
        {
            if (aSort != int.MaxValue)
            {
                return -1;
            }

            return a.CompareTo(b);
        }

        return aSort.CompareTo(bSort);
    });

    /// <summary>
    /// Returns a comparer that sorts <see cref="EMediaStatus"/> putting the more common types first.
    /// <see langword="null"/> values will be sorted last.
    /// </summary>
    /// <remarks>
    /// This comparer is primarily used for sorting statuses to be displayed.
    /// </remarks>
    public static IComparer<EMediaStatus?> NullableComparer { get; } = Comparer<EMediaStatus?>.Create(static (a, b) =>
    {
        if (!a.HasValue)
        {
            return b.HasValue ? 1 : 0;
        }

        if (!b.HasValue)
        {
            return a.HasValue ? -1 : 0;
        }

        return Comparer.Compare(a.Value, b.Value);
    });

    /// <summary>
    /// Gets the human friendly description/name of a <see cref="EMediaStatus"/>.
    /// </summary>
    /// <param name="status">The status to get the description/name of.</param>
    public static string GetDescription(this EMediaStatus status) => status switch
    {
        EMediaStatus.NotYetReleased => "Not Released",
        _ => status.ToString()
    };
}
