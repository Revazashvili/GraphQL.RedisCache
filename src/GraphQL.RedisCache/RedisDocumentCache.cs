using System.Text.Json;
using Forbids;
using GraphQL.Caching;
using GraphQLParser.AST;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace GraphQL.RedisCache;

public class RedisDocumentCache : IDocumentCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisDocumentCacheOptions _redisDocumentCacheOptions;
    
    public RedisDocumentCache(IDistributedCache distributedCache,IOptions<RedisDocumentCacheOptions> options)
    {
        _distributedCache = Forbid.From.Null(distributedCache);
        _redisDocumentCacheOptions = Forbid.From.Null(options.Value);
    }
    
    public async ValueTask<GraphQLDocument?> GetAsync(string query)
    {
        Forbid.From.NullOrEmpty(query);
        var cacheItemAsString = await _distributedCache.GetStringAsync(query);
        if (string.IsNullOrEmpty(cacheItemAsString))
            return null;
        
        var graphQlDocument = JsonSerializer.Deserialize<GraphQLDocument?>(cacheItemAsString);
        return graphQlDocument;
    }

    public async ValueTask SetAsync(string query, GraphQLDocument value)
    {
        Forbid.From.NullOrEmpty(query);
        Forbid.From.Null(value);

        var valueAsString = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(query, valueAsString, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = _redisDocumentCacheOptions.AbsoluteExpiration,
            SlidingExpiration = _redisDocumentCacheOptions.SlidingExpiration,
            AbsoluteExpirationRelativeToNow = _redisDocumentCacheOptions.AbsoluteExpirationRelativeToNow
        });
    }
}