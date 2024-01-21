namespace MiskCv_Api.Data.Repositories.UsersRepository;

public class UserRepository : IUserRepository
{
    private readonly MiskCvDbContext _context;

    public UserRepository(MiskCvDbContext context)
    {
        _context = context;
    }

    #region GET

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

    #endregion

    #region PUT

    public async Task<User?> UpdateUser(int id, User user)
    {
        if (_context.User == null) { return null; }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EntityExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return user;
    }

    #endregion

    #region POST

    public async Task<User?> CreateUser(User user)
    {
        if (_context.User == null) { return null; }

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteUser(int id)
    {
        if (_context.User == null) { return false; }

        var user = await _context.User.FindAsync(id);

        if (user == null) { return false; }

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    #endregion

    #region HELPERS

    private bool EntityExists(int id)
    {
        return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    #endregion
}
