using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.dto;

namespace UserServices.Validators
{
    public class CreateUserValidator : AbstractValidator<UserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(e => e.Login).MinimumLength(2).MaximumLength(20).WithMessage("must have more than 2 and less than 200 symbols");
            RuleFor(e => e.Password).MinimumLength(10).MaximumLength(20).WithMessage("must have more than 10 and less than 200 symbols");
        }
    }
}
