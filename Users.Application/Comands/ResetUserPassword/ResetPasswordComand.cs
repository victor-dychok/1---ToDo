using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Application.User.Application.Comands.UpdateUserPassword
{
    public class ResetPasswordComand : IRequest<GetUserDto>
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
}
