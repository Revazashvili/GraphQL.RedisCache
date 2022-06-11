using System.Text.Json;
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
        _distributedCache = distributedCache;
        _redisDocumentCacheOptions = options.Value;
    }
    
    public async ValueTask<GraphQLDocument?> GetAsync(string query)
    {
        var cacheItemAsString = await _distributedCache.GetStringAsync(query);
        if (string.IsNullOrEmpty(cacheItemAsString))
            return null;
        
        var graphQlDocument = JsonSerializer.Deserialize<GraphQLDocument?>(cacheItemAsString);
        return graphQlDocument;
    }

    public async ValueTask SetAsync(string query, GraphQLDocument value)
    {
        if (value is null)
            throw new ArgumentNullException();

        var valueAsString = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(query, valueAsString, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = _redisDocumentCacheOptions.AbsoluteExpiration,
            SlidingExpiration = _redisDocumentCacheOptions.SlidingExpiration,
            AbsoluteExpirationRelativeToNow = _redisDocumentCacheOptions.AbsoluteExpirationRelativeToNow
        });
    }
}