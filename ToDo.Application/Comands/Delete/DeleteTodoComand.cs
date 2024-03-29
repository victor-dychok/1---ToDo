using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDomain;

namespace ToDo.Application.Comands.Delete
{
    public class DeleteTodoComand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
