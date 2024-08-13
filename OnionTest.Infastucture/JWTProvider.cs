using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnionTest.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnionTest.Infastucture
{
    public class JWTProvider(IOptions<JwtOptions> options, IConfiguration configuration) : IJWTProvider
    {
        private readonly JwtOptions _options = options.Value;
        private readonly IConfiguration _configuration = configuration;


        public string GenerateToken(Users user)
        {
            Claim[] claims =
            {
                new("userId", user.Id.ToString()),
                new("Role", user.RoleId.ToString())
            };

            var secretKeyString = _configuration["SECRET_KEY"];
            if (secretKeyString == null)
            {
                throw new InvalidOperationException("SECRET_KEY is missing in configuration.");
            }

            var signingCredentails = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.ValidIssuer,
                audience: _options.ValidAudience,
                claims: claims,
                signingCredentials: signingCredentails,
                expires: DateTime.UtcNow.AddSeconds(_options.ExpitesHours));
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
