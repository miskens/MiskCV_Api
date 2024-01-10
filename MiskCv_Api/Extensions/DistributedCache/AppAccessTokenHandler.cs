using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MiskCv_Api.Services.AzureServices;

namespace MiskCv_Api.Extensions.DistributedCache;

public static class AppAccessTokenHandler
{
    public static async Task AddAppAccessTokenAsync(WebApplication app)
    {
        var authenticationService = app.Services.GetRequiredService<IAzureAuthenticationService>();
        Task<string> accessTokenTask = authenticationService.GetAccessTokenForAppAsync();
        var accessToken = accessTokenTask.Result;

        if (accessToken == null) { return; }

        var distributedCacheService = app.Services.GetRequiredService<IDistributedCache>();
        await SetAppAccessTokenAsync<string>(
            distributedCacheService,
            "appAccessToken", accessToken);
    }
    public static async Task SetAppAccessTokenAsync<T>(
                                this IDistributedCache cache,
                                string recordId,
                                T data)
    {
        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData);
    }
}
