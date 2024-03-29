using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Application.User.Application.Comands.UpdateUser
{
    public class UpdateUserComand : IRequest<GetUserDto>
    {
        public int Id { get; set; }
        public string Login { get; set; }
    }
}
