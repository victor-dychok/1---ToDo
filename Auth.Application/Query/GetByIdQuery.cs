using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace Auth.Application.Query
{
    public class GetByIdQuery : IRequest<GetUserDto>
    {
        public int Id { get; set; }
    }
}
