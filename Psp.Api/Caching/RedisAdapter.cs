using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Psp.Api.Caching;

public class RedisAdapter : ICaching
{
    private readonly IDistributedCache _cache;

    public RedisAdapter(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TValue?> GetAsync<TValue>(string key)
    {
        var value = await _cache.GetAsync(key);

        if (value is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<TValue>(value);
    }

    public async Task SetAsync<TValue>(string key, TValue value, TimeSpan expiration)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        var valueSerialized = JsonSerializer.Serialize(value);
        await _cache.SetAsync(key, Encoding.UTF8.GetBytes(valueSerialized), options);
    }
}
