using AutoMapper;
using OnionTest.Core.Models;
using OnionTest.DataAccess.Entites;

namespace OnionTest.Infastucture
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<UserEntity, Users>();
            CreateMap<RefreshTokenEntity, RefreshTokens>();
        }
    }
}