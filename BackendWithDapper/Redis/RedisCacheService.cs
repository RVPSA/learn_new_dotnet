using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }


    public T? GetData<T>(string key)
    {
        var data = _cache.GetString(key);
        if (data == null)
            return default(T);
        return JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T value)
    {
        var option = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
        };
        _cache.SetString(key, JsonSerializer.Serialize(value), option);
    }
}