namespace MiskCv_Api.Data.Repositories.UsersRepository;

public class UserRepository : IUserRepository
{
    private readonly MiskCvDbContext _context;

    public UserRepository(MiskCvDbContext context)
    {
        _context = context;
    }

    #region GET

    public async Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken)
    {
        if (_context.User == null)
        {
            return null;
        }

        var users = await _context.User.ToListAsync(cancellationToken);

        if (users.Count < 0 || users == null)
        {
            return null;
        }

        return users;
    }

    public async Task<User?> GetUser(int id, CancellationToken cancellationToken)
    {
        if (_context.User == null)
        {
            return null;
        }

        var user = await _context.User.FindAsync(id, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return user;
    }

    #endregion

    #region PUT

    public async Task<User?> UpdateUser(int id, User user, CancellationToken cancellationToken)
    {
        if (_context.User == null) { return null; }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
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

    public async Task<User?> CreateUser(User user, CancellationToken cancellationToken)
    {
        if (_context.User == null) { return null; }

        _context.User.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteUser(int id, CancellationToken cancellationToken)
    {
        if (_context.User == null) { return false; }

        var user = await _context.User.FindAsync(id, cancellationToken);

        if (user == null) { return false; }

        _context.User.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

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
