using Application.Users.Application.Query.GetAll;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.Query.GetById
{
    public class GetByIdValidator : AbstractValidator<GetByIdQuery>
    {
        public GetByIdValidator()
        {

        }
    }
}
