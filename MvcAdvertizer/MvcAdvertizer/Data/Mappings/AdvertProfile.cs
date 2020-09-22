using AutoMapper;
using MvcAdvertizer.Data.DTO;
using MvcAdvertizer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Data.Mappings
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile() {

            CreateMap<AdvertDto, Advert>();
        }
    }
}
