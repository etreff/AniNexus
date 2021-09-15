using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.GraphQL.Types;

/// <summary>
/// Defines all data about an anime.
/// </summary>
public class Anime
{
    /// <summary>
    /// The public AniNexus Id of the entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The category of the entry.
    /// </summary>
    public EAnimeCategory Category { get; set; }

    /// <summary>
    /// The English name of the entry.
    /// </summary>
    public string? EnglishName { get; set; }

    /// <summary>
    /// The Romaji name of the entry.
    /// </summary>
    public string? RomajiName { get; set; }

    /// <summary>
    /// The Japanese name of the entry.
    /// </summary>
    public string? KanjiName { get; set; }

    /// <summary>
    /// Aliases that the entry might have.
    /// </summary>
    public List<string>? Aliases { get; set; }

    /// <summary>
    /// Release information of the source entry.
    /// </summary>
    public AnimeReleaseInfo? ReleaseInfo { get; set; }

    /// <summary>
    /// A list of dubs this entry has.
    /// </summary>
    public List<AnimeReleaseInfo>? Dubs { get; set; }

    /// <summary>
    /// The definitive number of episodes this entry has.
    /// </summary>
    public short EpisodeCount { get; set; }

    /// <summary>
    /// The URL to the AniNexus cover art for this entry.
    /// </summary>
    public Uri? CoverArtUrl { get; set; }

    /// <summary>
    /// The official website URL of this entry.
    /// </summary>
    public Uri? WebsiteUrl { get; set; }

    /// <summary>
    /// The synopsis of the entry.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// A list of related entries.
    /// </summary>
    public List<AnimeRelatedEntry>? Related { get; set; }

    /// <summary>
    /// The user average rating of this entry.
    /// </summary>
    /// <remarks>
    /// An anime entry may not be eligible for a rating or may not
    /// have had a rating calculated for it yet. In these cases this
    /// value will be <see langword="null"/>.
    /// </remarks>
    public byte? Rating { get; set; }

    /// <summary>
    /// The number of users who contributed to the <see cref="Rating"/> average.
    /// </summary>
    public int Votes { get; set; }

    /// <summary>
    /// The number of user reviews this entry has.
    /// </summary>
    public short Reviews { get; set; }

    /// <summary>
    /// A list of external anime tracking websites and their Ids for this entry.
    /// </summary>
    /// <remarks>
    /// GraphQL does not have a Dictionary type, so the alternative we will use for
    /// this very simple case is a pipe (|) delimited string where the first part
    /// is the name of the website and the second part is the Id they have assigned
    /// this entry.
    /// </remarks>
    public List<string>? ExternalIds { get; set; }

    /// <summary>
    /// Tags assigned to this entry.
    /// </summary>
    public List<string>? Tags { get; set; }
}

public class AnimeType : ObjectType<Anime>
{
    protected override void Configure(IObjectTypeDescriptor<Anime> descriptor)
    {
        descriptor.Name("Anime");

        descriptor
            .Field(f => f.Id)
            .Description("The static Id of the anime entry.")
            .Type<IdType>();
        descriptor
            .Field(f => f.Category)
            .Description("The category of the anime.")
            .Type<EnumType<EAnimeCategory>>();
        descriptor
            .Field(f => f.EnglishName)
            .Description("The English name of the anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.RomajiName)
            .Description("The Romaji name of the anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.KanjiName)
            .Description("The kanji name of the anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.Aliases)
            .Description("A list of aliases that this anime may be known by.")
            .Type<ListType<StringType>>();
        descriptor
            .Field(f => f.ReleaseInfo)
            .Description("Release information about this anime.")
            .Type<AnimeReleaseInfoType>();
        descriptor
            .Field(f => f.Dubs)
            .Description("Release information about dubs for this anime.")
            .Type<ListType<AnimeReleaseInfoType>>();
        descriptor
            .Field(f => f.EpisodeCount)
            .Description("The total number of episodes this anime has.")
            .Type<ShortType>();
        descriptor
            .Field(f => f.CoverArtUrl)
            .Description("The URL to the anime cover art.")
            .Type<UrlType>();
        descriptor
            .Field(f => f.WebsiteUrl)
            .Description("The URL to the official anime website.")
            .Type<UrlType>();
        descriptor
            .Field(f => f.Synopsis)
            .Description("A synopsis of the anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.Related)
            .Description("A list of related anime.")
            .Type<ListType<AnimeRelatedEntryType>>();
        descriptor
            .Field(f => f.Rating)
            .Description("The anime rating from 0 to 100.")
            .Type<ByteType>();
        descriptor
            .Field(f => f.Votes)
            .Description("The number of users who rated this anime.")
            .Type<IntType>();
        descriptor
            .Field(f => f.Reviews)
            .Description("The number of reviews this anime has.")
            .Type<ShortType>();
        descriptor
            .Field(f => f.ExternalIds)
            .Description("A list of pipe-delimited entries that describe the Ids that third party websites have assigned this anime.")
            .Type<ListType<StringType>>();
        descriptor
            .Field(f => f.Tags)
            .Description("A list of tags for this anime.")
            .Type<ListType<StringType>>();
    }
}
