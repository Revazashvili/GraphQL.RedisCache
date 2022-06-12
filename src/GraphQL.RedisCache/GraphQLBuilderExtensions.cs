using Forbids;
using GraphQL.Caching;
using GraphQL.DI;

namespace GraphQL.RedisCache;

public static class GraphQLBuilderExtensions
{
    /// <summary>
    /// Registers <see cref="RedisDocumentCache"/> as a singleton of type <see cref="IDocumentCache"/> within the
    /// dependency injection framework.
    /// </summary>
    /// <param name="builder">The <see cref="IGraphQLBuilder"/>.</param>
    /// <param name="action">The configuration delegate.</param>
    public static IGraphQLBuilder AddRedisCache(this IGraphQLBuilder builder, Action<RedisDocumentCacheOptions> action)
    {
        Forbid.From.Null(action);
        builder.Services.Configure(action);
        return builder.AddDocumentCache<RedisDocumentCache>();
    }

    /// <inheritdoc cref="AddRedisCache(GraphQL.DI.IGraphQLBuilder,System.Action{GraphQL.RedisCache.RedisDocumentCacheOptions})"/>
    public static IGraphQLBuilder AddRedisCache(this IGraphQLBuilder builder,
        Action<RedisDocumentCacheOptions, IServiceProvider> action)
    {
        Forbid.From.Null(action);
        builder.Services.Configure(action);
        return builder.AddDocumentCache<RedisDocumentCache>();
    }
}