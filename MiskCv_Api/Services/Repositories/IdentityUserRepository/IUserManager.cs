using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api.Services.Repositories.IdentityUserRepository;

public interface IUserManager
{
    Task<IdentityUser?> FindByName(string userName);
    Task<IdentityResult>? CreateIdentityUserAsync(IdentityUser user);
}
