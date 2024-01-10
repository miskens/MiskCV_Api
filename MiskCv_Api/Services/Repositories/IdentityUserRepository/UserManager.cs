
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MiskCv_Api.Dtos.Identity;

namespace MiskCv_Api.Services.Repositories.IdentityUserRepository;

public class UserManager: IUserManager
{
    private readonly MiskIdentityDbContext _context;

    public UserManager(MiskIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IdentityUser?> FindByName(string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        if (user == null) { return null; } 

        return user;
    }

    public async Task<IdentityResult>? CreateIdentityUserAsync(IdentityUser user)
    {
        if(_context.Users == null) 
        { 
            return IdentityResult.Failed(); 
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return IdentityResult.Success;

    }
}
