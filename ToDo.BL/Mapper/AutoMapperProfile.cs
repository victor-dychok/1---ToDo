using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoBL.dto;
using ToDoDomain;

namespace ToDoBL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateToDo, TodoItem>();
            CreateMap<CreateToDo, TodoItem>();
        }
    }
}
