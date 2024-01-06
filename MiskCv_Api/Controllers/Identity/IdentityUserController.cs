using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MiskCv_Api.Dtos.Identity;
using MiskCv_Api.Services.Repositories.IdentityUserRepository;

namespace MiskCv_Api.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class IdentityUserController: ControllerBase
{
    private readonly IUserManager _userManager;

    public IdentityUserController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    #region Login

    //Login endpoint..

    #endregion

    #region Register

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if ((registerDto.Email != registerDto.ConfirmEmail || registerDto.Email.IsNullOrEmpty()))
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

        var result = await _userManager.CreateIdentityUserAsync(user, registerDto.Password)!;

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
