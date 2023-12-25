using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.UsersRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>?> GetUsers();
        Task<User?> GetUser(int id);
        Task<User?> UpdateUser(int id, User user);
    }
}
