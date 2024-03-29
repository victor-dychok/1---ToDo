using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.dto;
using ToDoDomain;

namespace ToDo.Application.Query.GetList
{
    public class GetListQuery : IRequest<IReadOnlyCollection<TodoItem>>
    {
        public int? Offset {  get; set; }
        public int? Limit { get; set; }
        public string? Lable { get; set; }
        public int? OwnerId { get; set; }
    }
}
