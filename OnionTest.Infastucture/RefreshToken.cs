using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnionTest.Infastucture
{
    public class RefreshToken : IRefreshToken
    {
        public Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
    }
}
