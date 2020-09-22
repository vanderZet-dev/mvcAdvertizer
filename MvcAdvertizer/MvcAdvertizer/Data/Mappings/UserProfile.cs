using AutoMapper;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;

namespace MvcAdvertizer.Data.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() {

            CreateMap<UserDto, User>();
        }
    }
}
