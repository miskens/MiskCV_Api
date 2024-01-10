using Microsoft.Identity.Client;

namespace MiskCv_Api.Services.AzureServices;

public class AzureAuthenticationService : IAzureAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IConfidentialClientApplication app;

    public AzureAuthenticationService(IConfiguration configuration)
    {
         app = ConfidentialClientApplicationBuilder.Create(configuration["AzureAd:ClientId"])
            .WithClientSecret(configuration["AzureAd:ClientSecret"])
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{configuration["AzureAd:TenantId"]}"))
            .Build();
        _configuration = configuration;
    }

    public async Task<string> GetAccessTokenForAppAsync()
    {
        var result = await app.AcquireTokenForClient(new[] { _configuration["AzureAd:AppIdUri"] }).ExecuteAsync();
        return result.AccessToken;
    }
}
