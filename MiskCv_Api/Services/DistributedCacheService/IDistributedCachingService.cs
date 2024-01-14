using Microsoft.Extensions.Caching.Distributed;

namespace MiskCv_Api.Services.DistributedCacheService;

public interface IDistributedCachingService
{
    Task<T> GetRecordAsync<T>(string recordId);
    Task SetRecordAsync<T>(string recordId, T data,
                            TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
}
