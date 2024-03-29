using AutoMapper;
using ToDo.Application.Comands.Create;
using ToDo.Application.Comands.Update;
using ToDo.Application.dto;
using ToDoDomain;

namespace ToDo.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateTodoComand, TodoItem>();
            CreateMap<CreateToDoComand, TodoItem>();
            CreateMap<TodoItem, TodoGetDto>();
        }
    }
}
