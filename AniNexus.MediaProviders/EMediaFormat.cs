using System.Collections.Immutable;

namespace AniNexus.MediaProviders;

/// <summary>
/// Defines a media format.
/// </summary>
public enum EMediaFormat
{
    /// <summary>
    /// Anime broadcast on television
    /// </summary>
    Tv,

    /// <summary>
    /// Anime which are under 15 minutes in length and broadcast on television
    /// </summary>
    TvShort,

    /// <summary>
    /// Anime movies with a theatrical release
    /// </summary>
    Movie,

    /// <summary>
    /// Special episodes that have been included in DVD/Blu-ray releases, picture dramas, pilots, etc
    /// </summary>
    Special,

    /// <summary>
    /// (Original Video Animation) Anime that have been released directly on DVD/Blu-ray without originally going through a theatrical release or television broadcast
    /// </summary>
    Ova,

    /// <summary>
    /// (Original Net Animation) Anime that have been originally released online or are only available through streaming services.
    /// </summary>
    Ona,

    /// <summary>
    /// Short anime released as a music video
    /// </summary>
    Music,

    /// <summary>
    /// Professionally published manga with more than one chapter
    /// </summary>
    Manga,

    /// <summary>
    /// Written books released as a series of light novels
    /// </summary>
    Novel,

    /// <summary>
    /// Manga with just one chapter
    /// </summary>
    OneShot
}

/// <summary>
/// A helper class for working with <see cref="EMediaFormat"/>.
/// </summary>
public static class MediaFormatHelper
{
    private static readonly IImmutableDictionary<EMediaFormat, int> _sortOrder = ImmutableDictionary.CreateRange(new KeyValuePair<EMediaFormat, int>[]
    {
        new(EMediaFormat.Tv, 0),
        new(EMediaFormat.Ova, 1),
        new(EMediaFormat.Special, 2),
        new(EMediaFormat.Ona, 3),
        new(EMediaFormat.Movie, 4)
    });

    /// <summary>
    /// Returns a comparer that sorts <see cref="EMediaFormat"/> putting the more common types first.
    /// </summary>
    /// <remarks>
    /// This comparer is primarily used for sorting formats to be displayed.
    /// </remarks>
    public static IComparer<EMediaFormat> Comparer { get; } = Comparer<EMediaFormat>.Create(static (a, b) =>
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
    /// Returns a comparer that sorts <see cref="EMediaFormat"/> putting the more common types first.
    /// <see langword="null"/> values will be sorted last.
    /// </summary>
    /// <remarks>
    /// This comparer is primarily used for sorting formats to be displayed.
    /// </remarks>
    public static IComparer<EMediaFormat?> NullableComparer { get; } = Comparer<EMediaFormat?>.Create(static (a, b) =>
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
    /// Gets the human friendly description/name of a <see cref="EMediaFormat"/>.
    /// </summary>
    /// <param name="format">The format to get the description/name of.</param>
    public static string GetDescription(this EMediaFormat format) => format switch
    {
        EMediaFormat.Movie => "Movies",
        EMediaFormat.Novel => "Novels",
        EMediaFormat.Ona => "ONAs",
        EMediaFormat.OneShot => "One Shots",
        EMediaFormat.Ova => "OVAs",
        EMediaFormat.Special => "Specials",
        EMediaFormat.Tv => "TV Episodes",
        EMediaFormat.TvShort => "TV Shorts",
        _ => format.ToString()
    };
}
