
namespace OnionTest.DataAccess.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task Add(string token, int userId);
        Task Delete(int id);
        Task DeleteAll();
        Task<bool> Exists(int userId);
        Task<bool> Get(int userId);
        Task Update(int userid, string token);
    }
}