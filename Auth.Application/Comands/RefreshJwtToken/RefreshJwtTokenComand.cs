using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Auth.Application.Comands.RefreshJwtToken
{
    public class RefreshJwtTokenComand : IRequest<JwtTokenDto>
    {
        public string RefreshToken { get; set; }
    }
}
