using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api.Services.Repositories.IdentityUserRepository;

public interface IUserManager
{
    Task<IdentityResult>? CreateIdentityUserAsync(IdentityUser user, string password);
}
