using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnionTest.Core.Models;
using OnionTest.DataAccess.Entites;

namespace OnionTest.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnionTestDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(OnionTestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Users user)
        {
            var userEntity = new UserEntity()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Barcode = user.Id + "_" + user.Email,
                RoleId = 2
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> GetByEmail(string email)
        {
            var userEntity = await _context.Users
               .AsNoTracking()
               .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception();

            return _mapper.Map<Users>(userEntity);

        }
        public async Task<Users> GetByid(int id)
        {
            var userEntity = await _context.Users
               .AsNoTracking()
               .FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception();

            return _mapper.Map<Users>(userEntity);

        }
        public async Task<bool> IsUserExistsByPartialEmailAsync(string partialEmail)
        {
            return await _context.Users.AnyAsync(user => user.Email.Contains(partialEmail));
        }

    }
}
