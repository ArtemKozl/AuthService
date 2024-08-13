
namespace OnionTest.Application.Services
{
    public interface IRefreshtokenService
    {
        Task<Dictionary<string, string>> Refresh(int id);
    }
}