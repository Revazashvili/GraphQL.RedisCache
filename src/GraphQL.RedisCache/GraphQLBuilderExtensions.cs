using GraphQL.Caching;
using GraphQL.DI;

namespace GraphQL.RedisCache;

public static class GraphQLBuilderExtensions
{
    /// <summary>
    /// Registers <see cref="AddRedisCache"/> as a singleton of type <see cref="IDocumentCache"/> within the
    /// dependency injection framework, and configures it with the specified configuration delegate.
    /// </summary>
    public static IGraphQLBuilder AddRedisCache(this IGraphQLBuilder builder,
        Action<RedisDocumentCacheOptions>? action = null)
    {
        builder.Services.Configure(action);
        return builder.AddDocumentCache<RedisDocumentCache>();
    }

    /// <inheritdoc cref="AddRedisCache(IGraphQLBuilder, Action{RedisDocumentCacheOptions})"/>
    public static IGraphQLBuilder AddMemoryCache(this IGraphQLBuilder builder,
        Action<RedisDocumentCacheOptions, IServiceProvider>? action)
    {
        builder.Services.Configure(action);
        return builder.AddDocumentCache<RedisDocumentCache>();
    }
}