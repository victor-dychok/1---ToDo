using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Query.GetByIdIsDone
{
    public class GetByIdIsDoneValidator : AbstractValidator<GetByIdIsDoneQuery>
    {
        public GetByIdIsDoneValidator() { }
    }
}
