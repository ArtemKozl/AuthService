
namespace OnionTest.Infastucture
{
    public interface IRefreshToken
    {
        Task<string> GenerateRefreshToken();
    }
}