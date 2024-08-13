using OnionTest.Core.Models;

namespace OnionTest.Infastucture
{
    public interface IJWTProvider
    {
        string GenerateToken(Users user);
    }
}