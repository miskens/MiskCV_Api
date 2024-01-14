using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MiskCv_Api.Services.AzureServices;

namespace MiskCv_Api.Services;

public static class AppAccessTokenHandler
{
    public static async Task AddAppAccessTokenAsync(WebApplication app)
    {
        var authenticationService = app.Services.GetRequiredService<IAzureAuthenticationService>();
        Task<string> accessTokenTask = authenticationService.GetAccessTokenForAppAsync();
        var accessToken = accessTokenTask.Result;

        if (accessToken == null) { return; }

        var distributedCacheService = app.Services.GetRequiredService<IDistributedCache>();
        await distributedCacheService.SetAppAccessTokenAsync(
            "appAccessToken", accessToken);
    }
    public static async Task SetAppAccessTokenAsync<T>(
                                this IDistributedCache cache,
                                string recordId,
                                T data)
    {
        var jsonData = JsonSerializer.Serialize(data);

        try
        {
            await cache.SetStringAsync(recordId, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unable to connect to Redis", ex);
        }

    }
}
