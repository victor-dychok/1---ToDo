using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Auth.Application.Comands.CreateJwtToken
{
    public class CreateJwtTokenComand : IRequest<JwtTokenDto>
    {
        public string Login {  get; set; }
        public string Password { get; set; }
    }
}
