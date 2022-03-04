using AniNexus.Data.GraphQL.Types;
using AniNexus.Data.GraphQL.Types.Primitives;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AniNexus.Data.GraphQL;

internal static class RequestExecutorBuilderExtensions
{
    public static IRequestExecutorBuilder AddAniNexusGraphQLTypes(this IRequestExecutorBuilder builder)
    {
        builder
            .AddQueryType<QueryType>()
            .AddPrimitiveTypes()
            .AddObjectTypes();

        return builder;
    }

    private static IRequestExecutorBuilder AddPrimitiveTypes(this IRequestExecutorBuilder builder)
    {
        builder
            .AddType<DateOnlyType>();

        return builder;
    }

    private static IRequestExecutorBuilder AddObjectTypes(this IRequestExecutorBuilder builder)
    {
        // Nested Queries
        //builder
        //    .AddType<AnimeQuery>();

        // Complex Types
        builder
            .AddType<AnimeType>()
            .AddType<AnimeRelatedEntryType>()
            .AddType<AnimeReleaseInfoType>();

        // Enums
        builder
            .AddType<AnimeCategoryType>()
            .AddType<AnimeRelationType>();

        return builder;
    }
}
