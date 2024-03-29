using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Comands.Create;
using ToDo.Application.Comands.Update;
using ToDo.Application.Comands.Patch;
using ToDo.Respounces;
using ToDo.Application.Comands.Delete;
using ToDo.Application.Query.GetList;
using ToDo.Application.Query.GetById;
using ToDo.Application.Query.GetByIdIsDone;

namespace ToDo.Controllers
{

    [Authorize]
    [ApiController]
    [Route("todos")]
    public class ToDoController : ControllerBase
    {
        public ToDoController() 
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetListQuery query, IMediator mediator, CancellationToken cancellationToken)
        {
            var todos = await mediator.Send(query, cancellationToken);
            HttpContext.Response.Headers.Append("X-Total-Count", todos.Count().ToString());
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdQuery query, IMediator mediator, CancellationToken cancellationToken) 
        {
            var item = await mediator.Send(query, cancellationToken);
            return Ok(item);
        }

        [HttpGet("{id:int}/IsDone")]
        public async Task<IActionResult> GetByIdIsDone([FromQuery] GetByIdIsDoneQuery query, IMediator mediator, CancellationToken cancellationToken)
        {
            var respItem = await mediator.Send(query, cancellationToken);
            var respounce = new ToDoIdFlagResp(respItem.Id, respItem.IsDone);
            return Ok(respounce);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateToDoComand item, IMediator mediator, CancellationToken cancellationToken)
        {
            var postedItem = await mediator.Send(item, cancellationToken);
            return Created($"/todos/{postedItem.Id}", postedItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateTodoComand comand, IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(comand, cancellationToken);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(item);
            }
        }

        [HttpPatch("{id:int}/IsDone")]
        public async Task<IActionResult> Putch(PatchTodoComand comand, IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(comand, cancellationToken);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                var respounce = new ToDoIdFlagResp(item.Id, item.IsDone);
                return Ok(respounce);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(new DeleteTodoComand { Id = id }, cancellationToken);
            return Ok(item);
        }
    }
}