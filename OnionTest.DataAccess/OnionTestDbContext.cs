using Microsoft.EntityFrameworkCore;
using OnionTest.DataAccess.Entites;

namespace OnionTest.DataAccess
{
    public class OnionTestDbContext : DbContext
    {
        public OnionTestDbContext(DbContextOptions<OnionTestDbContext> options)
            : base(options) 
        {
            
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}
