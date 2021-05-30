using AutoMapper;
using BL.Dtos;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
        

            //CreateMap<ApplicationUserIdentity, LogViewModel>().ReverseMap();
            CreateMap<ApplicationUserIdentity, RegisterViewModel>().ReverseMap();

           
        }
    }
}
