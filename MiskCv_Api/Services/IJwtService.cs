namespace MiskCv_Api.Services;

public interface IJwtService
{
    string GenerateToken(string userId, string username, string role = "USER", int expirationMinutes = 1);
}