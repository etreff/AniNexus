using AniNexus.Models;
using HotChocolate.Types;

namespace AniNexus.GraphQL.Types;

/// <summary>
/// Defines basic information about an anime that is related to another entry.
/// </summary>
public class AnimeRelatedEntry
{
    /// <summary>
    /// The public AniNexus Id of the entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The category of this entry.
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
    /// The relationship of this entry to the parent entry.
    /// </summary>
    public EMediaRelationType RelationType { get; set; }
}

public class AnimeRelatedEntryType : ObjectType<AnimeRelatedEntry>
{
    protected override void Configure(IObjectTypeDescriptor<AnimeRelatedEntry> descriptor)
    {
        descriptor.Name("RelatedAnime");

        descriptor
            .Field(f => f.Id)
            .Description("The static Id of the related anime entry.")
            .Type<IntType>();
        descriptor
            .Field(f => f.Category)
            .Description("The category of the related anime.")
            .Type<EnumType<EAnimeCategory>>();
        descriptor
            .Field(f => f.EnglishName)
            .Description("The English name of the related anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.RomajiName)
            .Description("The Romaji name of the related anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.KanjiName)
            .Description("The kanji name of the related anime.")
            .Type<StringType>();
        descriptor
            .Field(f => f.RelationType)
            .Description("How this anime is related to the parent entry.")
            .Type<EnumType<EMediaRelationType>>();
    }
}
