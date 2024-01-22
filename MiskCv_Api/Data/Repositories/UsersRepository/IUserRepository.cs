namespace MiskCv_Api.Data.Repositories.UsersRepository;

public interface IUserRepository
{
    Task<IEnumerable<User>?> GetUsers(CancellationToken cancellationToken);
    Task<User?> GetUser(int id, CancellationToken cancellationToken);
    Task<User?> UpdateUser(int id, User user, CancellationToken cancellationToken);
    Task<User?> CreateUser(User user, CancellationToken cancellationToken);
    Task<bool> DeleteUser(int id, CancellationToken cancellationToken);
}
