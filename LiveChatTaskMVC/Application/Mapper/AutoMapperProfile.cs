using AutoMapper;
using DTOs.ChatDTOs;
using DTOs.UserDTOs;

using Model;

namespace Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegisterDTO, AppUser>().ReverseMap();

            CreateMap<GetAllUserDTO, AppUser>().ReverseMap();
        }
    }
}
