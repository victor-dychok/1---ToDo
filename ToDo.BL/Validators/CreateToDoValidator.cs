using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoBL.dto;

namespace ToDoBL.Validators
{
    public class CreateToDoValidator : AbstractValidator<CreateToDo>
    {
        public CreateToDoValidator() 
        {
            RuleFor(e => e.OwnerId).GreaterThan(0).WithMessage("Owner id error");
            RuleFor(e=>e.Label).MinimumLength(10).MaximumLength(200).WithMessage("must have more than 10 and less than 200 symbols");
        }
    }
}
