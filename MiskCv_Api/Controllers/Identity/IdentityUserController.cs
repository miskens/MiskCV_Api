using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MiskCv_Api.Dtos.Identity;
using MiskCv_Api.Extensions.DistributedCache;
using MiskCv_Api.Services;
using MiskCv_Api.Services.Repositories.IdentityUserRepository;
using NuGet.Configuration;

namespace MiskCv_Api.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class IdentityUserController: ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtservice;
    private readonly IDistributedCache _cache;

    public IdentityUserController(
        IUserManager userManager, 
        IConfiguration configuration, 
        IJwtService jwtService, 
        IDistributedCache cache)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtservice = jwtService;
        _cache = cache;
    }

    #region Register

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if ((registerDto.Password != registerDto.ConfirmPassword || registerDto.Email.IsNullOrEmpty()))
        {
            return Problem("Email is empty or does not match confirm Email");
        }

        string hashedPassword = HashPassword(registerDto.Password);

        var user = new IdentityUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PasswordHash = hashedPassword
        };

        var result = await _userManager.CreateIdentityUserAsync(user)!;

        if(result == null)
        {
            return BadRequest("Failed to register User");
        }

        if (result.Succeeded)
        {
            return Ok("User registered successfully");
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    #endregion

    #region Login

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByName(loginDto.UserName);

        if (user == null || user.PasswordHash == null) { return NotFound(); }

        var authenticated = VerifyPassword(loginDto.Password, user.PasswordHash);

        if(!authenticated)
        {
            return Unauthorized("Incorrect password");
        }

        if (user.UserName != null)
        {
            var token = _jwtservice.GenerateToken(user.Id, user.UserName);

            if(token != null)
            {
                await _cache.SetRecordAsync<string>($"Jwt_User_{user.Id}", token);
                return Ok(token);
            } 
        }

        return Problem("Unable to login user.");
    }

    #endregion

    #region Logout

    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        // Implement token revocation

        // Clear server-side token data
        // ...

        return Ok(new { message = "Logout successful" });
    }

    #endregion

    #region Helpers

    public static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        string hashedInputPassword = HashPassword(inputPassword); // Hash the input password
        return StringComparer.OrdinalIgnoreCase.Compare(hashedInputPassword, hashedPassword) == 0;
    }

    #endregion
}
