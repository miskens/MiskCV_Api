using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Models;


namespace MiskCv_Api.Services.Repositories.UsersRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly MiskCvDbContext _context;

        public UserRepository(MiskCvDbContext context)
        {
            _context = context;
        }

      

        public async Task<IEnumerable<User>?> GetUsers()
        {
            if (_context.User == null)
            {
                return null;
            }

            var users = await _context.User.ToListAsync();

            if (users.Count < 0 || users == null)
            {
                return null;
            }

            return users;
        }  
        
        public async Task<User?> GetUser(int id)
        {
            if (_context.User == null)
            {
                return null;
            }

            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
