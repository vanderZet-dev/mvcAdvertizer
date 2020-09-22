﻿using AutoMapper;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;

namespace MvcAdvertizer.Data.Mappings
{
    public class AdvertDtoProfile : Profile
    {
        public AdvertDtoProfile() {

            var config = new MapperConfiguration(config => config.CreateMap<User, UserDto>());

            CreateMap<Advert, AdvertDto>()
                .ForMember(dst => dst.User, act => act.MapFrom(src=>src.User));                
        }
    }
}
