using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace GraphQL.RedisCache;

public class RedisDocumentCacheOptions : RedisCacheOptions
{
    public DateTimeOffset? AbsoluteExpiration { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
}