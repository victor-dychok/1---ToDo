using Application.Users.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Application.Query.GetCount
{
    public class GetCountQuery : BaseUsersFilter, IRequest<int>
    {
    }
}
