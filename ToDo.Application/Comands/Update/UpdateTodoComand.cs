using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.dto;
using ToDoDomain;

namespace ToDo.Application.Comands.Update
{
    public class UpdateTodoComand : IRequest<TodoGetDto>
    {
        public int Id { get; set; }
        public string? Label { get; set; } = default!;
        public bool IsDone { get; set; }
        public int OwnerId { get; set; }
    }
}
