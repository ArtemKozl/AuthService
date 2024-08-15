using OnionTest.Core.Models;
using OnionTest.DataAccess.Repositories;
using OnionTest.Infastucture;
using System.IdentityModel.Tokens.Jwt;

namespace OnionTest.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJWTProvider _jwtProvider;
        private readonly IRefreshToken _refreshToken;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IJWTProvider jwtProvider, IRefreshTokenRepository refershTokenRepository, IRefreshToken refreshToken)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refershTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _refreshToken = refreshToken;
        }
        public async Task Register(string username, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);
            var barcode = email + "_" + username;
            var newUser = Users.Create(username, email, hashedPassword, barcode);
            await _userRepository.Add(newUser);
        }
        public async Task<Dictionary<string, string>> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            var result = _passwordHasher.Verify(password, user.Password);
            if (result == false)
            {
                throw new Exception("Failed to Login");
            }

            var token = _jwtProvider.GenerateToken(user);

            var existsRefreshToken = await _refreshTokenRepository.Exists(user.Id);
            var refreshToken = await _refreshToken.GenerateRefreshToken();

            if (existsRefreshToken)
            {
                await _refreshTokenRepository.Update(user.Id, refreshToken);
            }
            else
            {
                await _refreshTokenRepository.Add(refreshToken, user.Id);
            }

            Dictionary<string, string> tokens = new()
            {
                { "access", token },
                { "refresh", refreshToken },
                { "UserName", user.UserName },
                { "Id", Convert.ToString(user.Id) }
            };

            return tokens;
        }

        public async Task<Users> GetUserByIdFromToken(string token)
        {
            try
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(Convert.ToString(token));

                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");

                if (userIdClaim != null)
                {
                    int userId = Convert.ToInt32(userIdClaim.Value);


                    Users user = await _userRepository.GetByid(userId);

                    return user;
                }
                else
                {
                    throw new Exception("User ID claim not found in the token.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing token: {ex.Message}");
                throw;
            }
        }


        public async Task<string> GetUserById(int id)
        {
            var user = await _userRepository.GetByid(id);
            return user.UserName;
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            return await _userRepository.IsUserExistsByPartialEmailAsync(email);
        }

    }
}
