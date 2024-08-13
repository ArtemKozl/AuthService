namespace OnionTest.Infastucture
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string pasword, string passwordHash);
    }
}