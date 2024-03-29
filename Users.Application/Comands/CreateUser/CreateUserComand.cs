
using MediatR;
using UserServices.dto;

namespace Application.Users.Application.Comands.CreateUser
{
    public class CreateUserComand : IRequest<GetUserDto>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
