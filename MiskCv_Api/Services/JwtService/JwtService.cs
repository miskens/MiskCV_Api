using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiskCv_Api.Services.JwtService;

public class JwtService : IJwtService
{
    private IDistributedCachingService _cache;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _revokedTokensKey = "revoked_tokens";

    public JwtService(IDistributedCachingService cache, IConfiguration config)
    {
        _cache = cache;
        _secretKey = config["Jwt:Key"]!;
        _issuer = config["Jwt:Issuer"]!;
        _audience = config["Jwt:Audience"]!;
    }

    public string GenerateToken(IdentityUser user, List<string> roles, int expirationMinutes = 5) //TODO: Change to 30 min
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        foreach (var role in roles)
        {
            var claim = new Claim(ClaimTypes.Role, role);
            claims.Add(claim);
        }

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> IsTokenRevoked(string token)
    {
        var revokedTokens = await _cache.GetRecordAsync<string>(_revokedTokensKey);

        var tokenList = JsonConvert.DeserializeObject<List<string>>(revokedTokens) ?? new List<string>();

        return tokenList.Contains(token);
    }

    public async Task RevokeToken(string userId, string token)
    {
        var revokedTokens = await _cache.GetRecordAsync<string>(_revokedTokensKey);

        var tokenList = new List<string>();
        if (revokedTokens != null)
            tokenList = JsonConvert.DeserializeObject<List<string>>(revokedTokens);

        tokenList!.Add(token);

        await _cache.SetRecordAsync(_revokedTokensKey, JsonConvert.SerializeObject(tokenList));
    }
}
