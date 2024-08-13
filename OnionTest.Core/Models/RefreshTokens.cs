using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionTest.Core.Models
{
    public class RefreshTokens
    {
        private RefreshTokens(int userId, string token, DateTime expareAt) 
        {
            UserId = userId;
            Token = token;
            ExpareAt = expareAt;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpareAt { get; set; }

        public static RefreshTokens Create(int userId, string token, DateTime expareAt)
        { 
            var refreshToken = new RefreshTokens(userId, token, expareAt);

            return refreshToken;
        }
    }
}
