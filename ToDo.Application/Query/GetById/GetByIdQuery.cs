using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDomain;

namespace ToDo.Application.Query.GetById
{
    public class GetByIdQuery : IRequest<TodoItem>
    {
        public int Id { get; set; }
    }
}
