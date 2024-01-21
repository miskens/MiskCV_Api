using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api.Dtos.Identity;

public record struct RegisterAdminDto(string UserName, string Email, string Role, string Password, string ConfirmPassword);
