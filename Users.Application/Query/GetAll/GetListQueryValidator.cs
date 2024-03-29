using Application.Users.Application.Query.GetAll;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Application.Query.GetAll
{
    public class GetListQueryValidator : AbstractValidator<GetListQuery>
    {
        public GetListQueryValidator()
        {
            RuleFor(x => x.Limit).GreaterThan(0).LessThan(int.MaxValue).When(x => x.Limit.HasValue);
            RuleFor(x => x.Offset).GreaterThan(0).LessThan(int.MaxValue).When(x => x.Offset.HasValue);
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }
}
