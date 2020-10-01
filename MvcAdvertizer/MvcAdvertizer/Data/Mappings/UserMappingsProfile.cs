using AutoMapper;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;

namespace MvcAdvertizer.Data.Mappings
{
    public class UserMappingsProfile : Profile
    {
        public UserMappingsProfile() {

            CreateMap<UserDto, User>();

            CreateMap<User, UserDto>();
        }
    }
}
