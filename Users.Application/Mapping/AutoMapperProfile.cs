using Application.Common.Domain;
using Application.User.Application.Comands.UpdateUser;
using Application.Users.Application.Comands.CreateUser;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace User.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<CreateUserComand, AppUser>();

            CreateMap<GetUserDto, AppUser>();
            CreateMap<AppUser, GetUserDto>();

            CreateMap<UpdateUserComand, AppUser>();
        }
    }
}
