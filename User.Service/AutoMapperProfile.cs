using AutoMapper;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Service.dto;
using UserServices.dto;

namespace UserServices
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UserDto, AppUser>();

            CreateMap<UserGetDto, AppUser>();
            CreateMap<AppUser, UserGetDto>();

            CreateMap<UserPutDto, AppUser>();
        }
    }
}
