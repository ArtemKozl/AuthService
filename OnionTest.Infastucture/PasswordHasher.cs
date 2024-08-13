using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionTest.Infastucture
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string pasword, string passwordHash) =>
            BCrypt.Net.BCrypt.EnhancedVerify(pasword, passwordHash);
    }
}
