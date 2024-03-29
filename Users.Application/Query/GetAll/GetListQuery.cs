using Application.Users.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoDomain;
using UserServices.dto;

namespace Application.Users.Application.Query.GetAll
{
    public class GetListQuery : BaseUsersFilter, IRequest<IReadOnlyCollection<GetUserDto>>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
