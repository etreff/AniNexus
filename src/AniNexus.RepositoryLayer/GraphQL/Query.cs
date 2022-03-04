using AniNexus.Domain;
using AniNexus.Domain.Models;
using AniNexus.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Data.GraphQL;

public class Query
{
    [UseDbContext(typeof(ApplicationDbContext))]
    public IQueryable<AnimeModel> GetAnime([ScopedService] ApplicationDbContext context)
    {
        return context.Anime.AsNoTracking();
    }

    //public Task<AnimeModel?> GetAnime(int id, AnimeBatchDataLoader dataLoader)
    //    => dataLoader.LoadAsync(id);
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field(f => f.GetAnime(default!))
            .Description("Anime-related queries.")
            .Type<ListType<NonNullType<AnimeType>>>()
            .UseFiltering();
    }
}

public class AnimeType : ObjectType<AnimeModel>
{
    protected override void Configure(IObjectTypeDescriptor<AnimeModel> descriptor)
    {
        descriptor.Name("Anime");

        descriptor.BindFieldsExplicitly();

        descriptor.Field(f => f.ActiveRating).Type<ByteType>().Description("The rating of the series calcuated only by scores provided from users who are watching the anime.");
        descriptor.Field(f => f.CategoryId).Type<EnumType<EAnimeCategory>>().Description("The category of the anime.");
        descriptor.Field(f => f.Characters);
        descriptor.Field(f => f.Companies);
        descriptor.Field(f => f.DateAdded).Type<DateTimeType>().Description("The date this entry was added.");
        descriptor.Field(f => f.DateUpdated).Type<DateTimeType>().Description("The date this entry was last updated.");
        descriptor.Field(f => f.ExternalIds);
        descriptor.Field(f => f.Favorites);
        descriptor.Field(f => f.Genres);
        descriptor.Field(f => f.Id).Type<IdType>().Description("The AniNexus Id of the anime.");
        descriptor.Field(f => f.People);
        descriptor.Field(f => f.Rating).Type<ByteType>().Description("The rating of the series calculated only by the scores provided from users who have completed the anime.");
        descriptor.Field(f => f.Related);
        descriptor.Field(f => f.Releases);
        descriptor.Field(f => f.SeasonId).Type<EnumType<EAnimeSeason>>().Description("The anime season.");
        descriptor.Field(f => f.Series);
        descriptor.Field(f => f.Synopsis).Type<StringType>().Description("A synopsis of the anime.");
        descriptor.Field(f => f.Tags);
        descriptor.Field(f => f.TwitterHashtags);
        descriptor.Field(f => f.Votes).Type<IntType>().Description("The number of users who have rated the anime (completed only).");
        descriptor.Field(f => f.WebsiteUrl).Type<StringType>().Description("A URL to the anime's official website.");
    }
}

//public class AnimeBatchDataLoader : BatchDataLoader<int, AnimeModel?>
//{
//    private readonly IRepositoryProvider RepositoryProvider;

//    public AnimeBatchDataLoader(IRepositoryProvider repositoryProvider, IBatchScheduler batchScheduler, DataLoaderOptions<int>? options = null)
//        : base(batchScheduler, options)
//    {
//        RepositoryProvider = repositoryProvider;
//    }

//    protected override async Task<IReadOnlyDictionary<int, AnimeModel?>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
//    {
//        //var animeRepository = RepositoryProvider.GetAnimeRepository();
//        //var anime = await animeRepository.GetAnimeAsync(keys, cancellationToken);

//        //var dictionary = new Dictionary<int, AnimeModel?>(keys.Count);
//        //foreach (int key in keys)
//        //{
//        //    dictionary.Add(key, anime.FirstOrDefault(a => a.Id == key));
//        //}

//        //return dictionary;

//        return new Dictionary<int, AnimeModel?>();
//    }
//}
