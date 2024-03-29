using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.dto;
using ToDoDomain;

namespace ToDo.Application.Comands.Patch
{
    public class PatchTodoComand : IRequest<TodoGetDto>
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
    }
}
