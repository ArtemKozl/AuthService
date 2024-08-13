using OnionTest.Core.Models;

namespace OnionTest.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task Add(Users user);
        Task<Users> GetByEmail(string email);
        Task<Users> GetByid(int id);
        Task<bool> IsUserExistsByPartialEmailAsync(string partialEmail);
    }
}