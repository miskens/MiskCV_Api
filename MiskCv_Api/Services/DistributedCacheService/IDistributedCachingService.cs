using Microsoft.Extensions.Caching.Distributed;

namespace MiskCv_Api.Services.DistributedCacheService;

public interface IDistributedCachingService
{
    Task<T> GetRecordAsync<T>(string recordId, CancellationToken cancellationToken);
    Task SetRecordAsync<T>(string recordId, T data, CancellationToken cancellationToken,
                            TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
}
