using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Comands.Delete;

namespace ToDo.Application.Comands.Patch
{
    public class PutchTodoComandValidator : AbstractValidator<PatchTodoComand>
    {
        public PutchTodoComandValidator() { }
    }
}
