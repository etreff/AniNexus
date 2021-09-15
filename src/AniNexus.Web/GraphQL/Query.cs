using AniNexus.GraphQL.Queries;
using HotChocolate.Types;

namespace AniNexus.GraphQL;

public class QueryType : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("anime")
            .Description("Anime-related queries.")
            .Type<AnimeQuery>()
            .Resolver(AnimeQuery.Instance);
    }
}