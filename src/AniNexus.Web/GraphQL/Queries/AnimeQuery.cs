using AniNexus.Domain;
using AniNexus.GraphQL.Types;
using HotChocolate.Types;

namespace AniNexus.GraphQL.Queries;

public class AnimeQuery : ObjectType
{
    public static AnimeQuery Instance { get; } = new AnimeQuery();

    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("id")
            .Description("Finds an anime by its static Id and returns its information.")
            .Type<AnimeType>()
            .Argument("id", a => a.Type<IntType>())
            .UseDbContext<ApplicationDbContext>()
            .Resolve((context, token) =>
            {
                //int id = context.ArgumentValue<int>("id");
                //var db = context.Service<IDbContextFactory<ApplicationDbContext>>().CreateDbContext();
                //var coverArtService = context.Service<IAnimeCoverArtService>();

                //var anime = await AnimeModel.IncludeAll(db.Anime)
                //    .FirstOrDefaultAsync(a => a.Id == id, token);
                //return await MapAnimeAsync(anime, coverArtService, token);

                return null;
            });
        descriptor
            .Field("name")
            .Description("Finds an anime by its name Id and returns its information.")
            .Argument("name", a => a.Type<NonNullType<StringType>>())
            .Type<AnimeType>()
            .UseDbContext<ApplicationDbContext>()
            .Resolve((context, token) =>
            {
                //string name = context.ArgumentValue<string>("name");
                //var db = context.Service<ApplicationDbContext>();
                //var coverArtService = context.Service<IAnimeCoverArtService>();

                //var release = await db.AnimeReleases
                //    .Where(r => r.Name.EnglishName == name ||
                //                r.Name.RomajiName == name ||
                //                r.Name.NativeName == name)
                //    .FirstOrDefaultAsync(token);

                //if (release is null)
                //{
                //    return null;
                //}

                //var anime = await AnimeModel.IncludeAll(db.Anime)
                //    .FirstAsync(a => a.Id == release.AnimeId, token);

                //return await MapAnimeAsync(anime, coverArtService, token);

                return null;
            });
        descriptor
            .Field("find")
            .Description("Finds anime that are similar in name to the specified value and returns information about the results.")
            .Argument("name", a => a.Type<NonNullType<StringType>>())
            .Type<ListType<AnimeType>>()
            .UseDbContext<ApplicationDbContext>()
            .Resolve((context, token) =>
            {
                //string name = context.ArgumentValue<string>("name");
                //var db = context.Service<ApplicationDbContext>();
                //var coverArtService = context.Service<IAnimeCoverArtService>();

                //var releases = await db.AnimeReleases
                //    .Where(r => (r.Name.EnglishName != null && r.Name.EnglishName.Contains(name)) ||
                //                (r.Name.RomajiName != null && r.Name.RomajiName.Contains(name)) ||
                //                (r.Name.NativeName != null && r.Name.NativeName.Contains(name)))
                //    .ToListAsync(token);

                //if (releases.Count == 0)
                //{
                //    return null;
                //}

                //var result = new Dictionary<int, Anime>(releases.Count);
                //foreach (var release in releases)
                //{
                //    //TODO: This doesn't seem remotely performant, but if the search is only 8-10 results we can leave it for now.
                //    var anime = await AnimeModel.IncludeAll(db.Anime)
                //        .FirstAsync(a => a.Id == release.AnimeId, token);

                //    result.TryAdd(anime.Id, (await MapAnimeAsync(anime, coverArtService, token))!);
                //}

                //return result.Values.ToList();

                return null;
            });
        descriptor
            .Field("airing")
            .Description("Returns information about the anime that are currently airing.")
            .Type<ListType<AnimeType>>()
            .UseDbContext<ApplicationDbContext>()
            .Resolve((context, token) =>
            {
                //var db = context.Service<ApplicationDbContext>();
                //var airingIds = await db.AnimeAiring.Select(a => a.Release.AnimeId).ToListAsync();

                //var data = await AnimeModel.IncludeAll(db.Anime)
                //    .Where(a => airingIds.Contains(a.Id))
                //    .ToListAsync(token);

                //var coverArtService = context.Service<IAnimeCoverArtService>();

                //var result = new List<Anime>(data.Count);
                //foreach (var anime in data)
                //{
                //    result.Add((await MapAnimeAsync(anime, coverArtService, token))!);
                //}
                //return result;

                return null;
            });
    }

    //private static ValueTask<Anime?> MapAnimeAsync(AnimeModel? anime, IAnimeCoverArtService coverArtService, CancellationToken cancellationToken)
    //{
    //    return new ValueTask<Anime?>((Anime?)null);

    //    if (anime is null)
    //    {
    //        return null;
    //    }

    //    return new Anime
    //    {
    //        Aliases = anime.Aliases?.Select(x => x.Name).ToList(),
    //        CoverArtUrl = await coverArtService.GetCoverArtUrlAsync(anime.Id, cancellationToken),
    //        Category = (EAnimeCategory)anime.Category.Id,
    //        Dubs = anime.Releases?.Where(r => !r.IsPrimary).Select(r => new AnimeReleaseInfo
    //        {
    //            IsAdult = r.IsAdult,
    //            EndDate = r.EndDate,
    //            LanguageCode = r.Locale.LanguageCode,
    //            LatestEpisodeCount = r.LatestEpisodeCount,
    //            StartDate = r.StartDate,
    //            Stations = r.Stations?.ToList()
    //        }).ToList(),
    //        EnglishName = anime.EnglishName,
    //        EpisodeCount = anime.EpisodeCount ?? 0,
    //        ExternalIds = anime.ExternalIds?.Select(x => $"{x.ThirdParty.Name}|{x.ExternalMediaId}").ToList(),
    //        Id = anime.Id,
    //        KanjiName = anime.KanjiName,
    //        Rating = anime.Rating,
    //        Related = anime.RelatedEntries?.Select(r => new AnimeRelatedEntry
    //        {
    //            Category = (EAnimeCategory)r.Media.Category.Id,
    //            EnglishName = r.Media.EnglishName,
    //            Id = r.Media.Id,
    //            KanjiName = r.Media.KanjiName,
    //            RelationType = (EAnimeRelationType)r.Relation.Id,
    //            RomajiName = r.Media.RomajiName
    //        }).ToList(),
    //        ReleaseInfo = anime.Releases?.Where(r => r.IsPrimary).Select(r => new AnimeReleaseInfo
    //        {
    //            IsAdult = r.IsAdult,
    //            EndDate = r.EndDate,
    //            LanguageCode = r.Locale.LanguageCode,
    //            LatestEpisodeCount = r.LatestEpisodeCount,
    //            StartDate = r.StartDate,
    //            Stations = r.Stations?.ToList()
    //        }).FirstOrDefault(),
    //        Reviews = 0,
    //        RomajiName = anime.RomajiName,
    //        Synopsis = anime.Synopsis,
    //        Tags = anime.ContentTags?.Select(r => r.Genre.Name).ToList(),
    //        Votes = anime.Votes,
    //        WebsiteUrl = Uri.TryCreate(anime.WebsiteUrl, UriKind.Absolute, out var websiteUrl) ? websiteUrl : null,
    //    };
    //}
}
