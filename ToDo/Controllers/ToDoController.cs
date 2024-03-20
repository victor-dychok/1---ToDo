using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.BL;
using ToDo.Respounces;
using ToDoBL;
using ToDoBL.dto;

namespace ToDo.Controllers
{

    [Authorize]
    [ApiController]
    [Route("todos")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        public ToDoController(IToDoService service) 
        {
            _toDoService = service;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetAll(int? offset, int? ownerId, string? lable, int? limit)
        {
            var todos = await _toDoService.GetListAsync(offset, ownerId, lable, limit);
            HttpContext.Response.Headers.Append("X-Total-Count", todos.Count().ToString());
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) 
        {

            var item = await _toDoService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            else return Ok(item);
        }

        [HttpGet("{id:int}/IsDone")]
        public IActionResult GetByIdIsDone(int id, CancellationToken cancellationToken)
        {
            var respItem = _toDoService.GetByIdIsDoneAsync(id, cancellationToken);
            var respounce = new ToDoIdFlagResp(respItem.Id, respItem.Result.IsDone);
            return Ok(respounce);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateToDo item)
        {
            var postedItem = await _toDoService.AddAsync(item);
            if(postedItem != null)
            {
                return Created("/todos", item);
            }
            else return BadRequest();


        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateToDo newItem)
        {
            var item = await _toDoService.UpdateAsync(newItem);

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
        public async Task<IActionResult> Putch(int id, bool isDone)
        {
            var item = await _toDoService.PutchAsync(id, isDone);
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
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _toDoService.DeleteAsync(id);
            return Ok(item);
        }
    }
}