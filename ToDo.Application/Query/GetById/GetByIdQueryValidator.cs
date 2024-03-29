using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Query.GetById
{
    public class GetByIdIsDoneQueryValidator : AbstractValidator<GetByIdQuery>
    {
        public GetByIdIsDoneQueryValidator() { }
    }
}
