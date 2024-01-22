using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace MiskCv_Api.Services.DistributedCacheService;

public class DistributedCachingService : IDistributedCachingService
{
    private IConfiguration? _config;
    private IDistributedCache? _cache;

    public DistributedCachingService(IConfiguration config, IDistributedCache cache)
    {
        _config = config;
        _cache = cache;
    }

    public async Task<T> GetRecordAsync<T>(string recordId, CancellationToken cancellationToken)
    {
        if (_cache == null) throw new ArgumentNullException(nameof(_cache));

        var jsonData = await _cache.GetStringAsync(recordId, cancellationToken);

        if (jsonData == null) return default!;

        return JsonSerializer.Deserialize<T>(jsonData)!;
    }

    public async Task SetRecordAsync<T>(
                                string recordId,
                                T data,
                                CancellationToken cancellationToken,
                                TimeSpan? absoluteExpireTime = null,
                                TimeSpan? unusedExpireTime = null)
    {
        var absoluteExpireTimeDefault = _config!.GetSection("Redis:AbsoluteExpiration").Get<double>();
        var slidingExpireTimeDefault = _config!.GetSection("Redis:SlidingExpiration").Get<double>();

        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(absoluteExpireTimeDefault);
        options.SlidingExpiration = unusedExpireTime ?? TimeSpan.FromSeconds(slidingExpireTimeDefault);

        var jsonData = JsonSerializer.Serialize(data);

        if (_cache != null)
            await _cache.SetStringAsync(recordId, jsonData, options, cancellationToken);
    }
}
