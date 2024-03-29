using Application.Users.Application.Comands.CreateUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Application.Comands.CreateUser
{
    public class CreateUserComandValidator : AbstractValidator<CreateUserComand>
    {
        public CreateUserComandValidator()
        {
            RuleFor(e => e.Login).MinimumLength(2).MaximumLength(20).WithMessage("must have more than 2 and less than 200 symbols");
            RuleFor(e => e.Password).MinimumLength(10).MaximumLength(20).WithMessage("must have more than 10 and less than 200 symbols");
        }
    }
}
