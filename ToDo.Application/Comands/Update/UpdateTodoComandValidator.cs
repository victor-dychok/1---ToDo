using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Comands.Update
{
    public class UpdateTodoComandValidator : AbstractValidator<UpdateTodoComand>
    {
        public UpdateTodoComandValidator() { }
    }
}
