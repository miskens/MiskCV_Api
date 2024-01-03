using System.Text.Json;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Protocols;

namespace MiskCv_Api.Extensions;

public static class DistributedCacheExtension
{
    private static IConfiguration? _config { get; set; }

    public static void SetConfiguration(IConfiguration config)
    {
        _config = config;
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        var jsonData = await cache.GetStringAsync(recordId);

        if (jsonData == null) return default(T)!;

        return JsonSerializer.Deserialize<T>(jsonData)!;
    }

    public static async Task SetRecordAsync<T>(
                                this IDistributedCache cache, 
                                string recordId, 
                                T data, 
                                TimeSpan? absoluteExpireTime = null,
                                TimeSpan? unusedExpireTime = null)
    {
        var absoluteExpireTimeDefault = _config!.GetSection("Redis:AbsoluteExpiration").Get<double>();
        var slidingExpireTimeDefault = _config!.GetSection("Redis:SlidingExpiration").Get<double>();

        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(absoluteExpireTimeDefault);
        options.SlidingExpiration = unusedExpireTime ?? TimeSpan.FromSeconds(slidingExpireTimeDefault); 

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }
}
