using OnionTest.Core.Models;

namespace OnionTest.Application.Services
{
    public interface IUserService
    {
        Task<string> GetUserById(int id);
        Task<Users> GetUserByIdFromToken(string token);
        Task<Dictionary<string, string>> Login(string email, string password);
        Task Register(string username, string email, string password);
        Task<bool> UserExistsByEmail(string email);
    }
}