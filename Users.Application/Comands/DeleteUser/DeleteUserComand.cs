using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Comands.DeleteUser
{
    public class DeleteUserComand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
