using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Comands.Create;

namespace ToDo.Application.Comands.Delete
{
    public class DeleteToDoComandValidator : AbstractValidator<DeleteTodoComand>
    {
        public DeleteToDoComandValidator() { }
    }
}
