using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using MiskCv_Api.Dtos.Identity;
using MiskCv_Api.Services;
using MiskCv_Api.Services.DistributedCacheService;
using StackExchange.Redis;

namespace MiskCv_Api.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class IdentityUserController: ControllerBase
{
    //private readonly IUserManager _userManager;
    private UserManager<IdentityUser> _userManager;
    private readonly IJwtService _jwtservice;
    private readonly IDistributedCachingService _cache;

    public IdentityUserController(
        UserManager<IdentityUser> userManager,
        IJwtService jwtService,
        IDistributedCachingService cache)
    {
        _userManager = userManager;
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

        var result = await _userManager.CreateAsync(user)!;
        await _userManager.AddToRoleAsync(user, "USER");

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
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null || user.PasswordHash == null) { return NotFound(); }

        var authenticated = VerifyPassword(loginDto.Password, user.PasswordHash);

        if(!authenticated)
        {
            return Unauthorized("Incorrect password");
        }

        if (user.UserName != null)
        {
            var token = _jwtservice.GenerateToken(user.Id, user.UserName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

            if (token != null)
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
        var user = await _userManager.FindByNameAsync(User!.Identity!.Name!);
        
        if(user != null)
        {
            var token = await _cache.GetRecordAsync<string>($"Jwt_User_{user.Id}");

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            await _jwtservice.RevokeToken(user.Id, token);

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            return Ok(new { message = "Logout successful" });
        }

        return BadRequest($"Logout failed for user: {User.Identity.Name}");
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
