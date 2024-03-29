using Application.Common.Domain;
using Auth.Api.dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Auth.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthDto, AppUser>();
            CreateMap<AppUser, AuthDto>();
            CreateMap<AppUser, GetUserDto>();
        }
    }
}
