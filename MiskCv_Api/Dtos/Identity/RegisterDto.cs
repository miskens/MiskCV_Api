using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api.Dtos.Identity;

public record struct RegisterDto(string UserName, string Email, string Password, string ConfirmPassword);
