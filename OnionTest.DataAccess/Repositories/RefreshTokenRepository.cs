using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnionTest.DataAccess.Entites;

namespace OnionTest.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly OnionTestDbContext _context;
        private readonly IMapper _mapper;

        public RefreshTokenRepository(OnionTestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(string token, int userId)
        {
            var refreshTokenEntity = new RefreshTokenEntity()
            {
                UserId = userId,
                Token = token,
                ExpireAt = DateTime.Now.AddDays(7).ToUniversalTime()
            };
            await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> Get(int userId)
        {
            bool result = false;

            var refreshTokenEntity = await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (refreshTokenEntity != null && refreshTokenEntity.ExpireAt > DateTime.UtcNow)
            {
                result = true;
            }

            return result;
        }

        public async Task Delete(int id)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (refreshTokenEntity != null)
                _context.RefreshTokens.Remove(refreshTokenEntity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            var tokens = await _context.RefreshTokens.AsNoTracking().ToListAsync();

            _context.RefreshTokens.RemoveRange(tokens);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> Exists(int userId)
        {
            bool exists = await _context.RefreshTokens.AnyAsync(e => e.UserId == userId);

            return exists;
        }

        public async Task Update(int userid, string token)
        {

            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(u => u.UserId == userid);

            if (refreshTokenEntity != null)
            {
                refreshTokenEntity.ExpireAt = DateTime.Now.AddDays(7).ToUniversalTime();
                refreshTokenEntity.Token = token;

                await _context.SaveChangesAsync();
            }

        }
    }
}
