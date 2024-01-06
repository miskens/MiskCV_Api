
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MiskCv_Api.Services.Repositories.IdentityUserRepository;

public class UserManager: IUserManager
{
    private readonly MiskIdentityDbContext _context;

    public UserManager(MiskIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IdentityResult>? CreateIdentityUserAsync(IdentityUser user, string password)
    {
        if(_context.Users == null
           || user == null
           || password.IsNullOrEmpty()) 
        { 
            return IdentityResult.Failed(); 
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return IdentityResult.Success;

    }
}
