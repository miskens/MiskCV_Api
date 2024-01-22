using System.Threading;
using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api.Services.JwtService;

public interface IJwtService
{
    string GenerateToken(IdentityUser user, List<string> roles, int expirationMinutes = 1);
    Task<bool> IsTokenRevoked(string token, CancellationToken cancellationToken);
    Task RevokeToken(string userId, string token, CancellationToken cancellationToken);
}