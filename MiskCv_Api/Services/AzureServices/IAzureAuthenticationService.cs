namespace MiskCv_Api.Services.AzureServices;

public interface IAzureAuthenticationService
{
    Task<string> GetAccessTokenForAppAsync();
}
