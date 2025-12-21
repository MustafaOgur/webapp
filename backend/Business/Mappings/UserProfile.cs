using AutoMapper;
using Model.DTOs.User;
using Core.Entities;
using Model.DTOs.Chat;
using Model.Entities;

namespace Business.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<LoginDto, User>();
            CreateMap<User, UserDto>();

        }
    }
}
