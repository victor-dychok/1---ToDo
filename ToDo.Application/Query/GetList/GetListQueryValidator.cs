using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Query.GetList
{
    public class GetListQueryValidator : AbstractValidator<GetListQuery>
    {
        public GetListQueryValidator()
        {
            RuleFor(x => x.Limit).GreaterThan(0).LessThan(int.MaxValue).When(x => x.Limit.HasValue);
            RuleFor(x => x.Offset).GreaterThan(0).LessThan(int.MaxValue).When(x => x.Offset.HasValue);
            RuleFor(x => x.Lable).MaximumLength(100);
        }
    }
}
