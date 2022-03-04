using AniNexus.Data.GraphQL.Types.Primitives;
using HotChocolate.Types;

namespace AniNexus.Data.GraphQL.Types;

/// <summary>
/// Defines information about an anime's release.
/// </summary>
public class AnimeReleaseInfo
{
    /// <summary>
    /// The country in which this version of the anime was released.
    /// </summary>
    public string? LanguageCode { get; set; }

    /// <summary>
    /// The air date of the first episode in the country.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The air date of the last episode in the country.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// The number of episodes that have currently been released.
    /// </summary>
    public short LatestEpisodeCount { get; set; }

    /// <summary>
    /// A list of stations that this anime aired on.
    /// </summary>
    public List<string>? Stations { get; set; }

    /// <summary>
    /// Whether this entry is rated Adult in their country.
    /// </summary>
    public bool IsAdult { get; set; }
}

public class AnimeReleaseInfoType : ObjectType<AnimeReleaseInfo>
{
    protected override void Configure(IObjectTypeDescriptor<AnimeReleaseInfo> descriptor)
    {
        descriptor.Name("AnimeRelease");

        descriptor
            .Field(f => f.LanguageCode)
            .Description("The language code of this release.")
            .Type<StringType>();
        descriptor
            .Field(f => f.StartDate)
            .Description("The date the first element of this entry was released.")
            .Type<DateOnlyType>();
        descriptor
            .Field(f => f.EndDate)
            .Description("The date the last element of this entry was released.")
            .Type<DateOnlyType>();
        descriptor
            .Field(f => f.LatestEpisodeCount)
            .Description("The number of episodes that have been released so far in this release.")
            .Type<ShortType>();
        descriptor
            .Field(f => f.Stations)
            .Description("The TV stations or other release avenues that this release was released on.")
            .Type<ListType<StringType>>();
        descriptor
            .Field(f => f.IsAdult)
            .Description("Whether this entry is rated Adult in the region of this release.")
            .Type<BooleanType>();
    }
}