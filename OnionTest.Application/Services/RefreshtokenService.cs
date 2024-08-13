using OnionTest.Core.Models;
using OnionTest.DataAccess.Repositories;
using OnionTest.Infastucture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionTest.Application.Services
{
    public class RefreshtokenService : IRefreshtokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJWTProvider _jwtProvider;
        private readonly IRefreshToken _refreshToken;

        public RefreshtokenService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
            IJWTProvider jwtProvider, IRefreshToken refreshToken)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _refreshToken = refreshToken;
        }
        public async Task<Dictionary<string, string>> Refresh(int id)
        {
            var user = await _userRepository.GetByid(id);

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
                { "refresh", refreshToken }
            };

            return tokens;
        }

    }
}
