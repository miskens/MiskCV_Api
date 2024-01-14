namespace MiskCv_Api.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string username, string role = "USER", int expirationMinutes = 1);
    Task<bool> IsTokenRevoked(string token);
    Task RevokeToken(string userId, string token);
}