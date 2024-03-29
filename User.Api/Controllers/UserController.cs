using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.User.Application.Comands.DeleteUser;
using Application.User.Application.Comands.UpdateUser;
using Application.User.Application.Comands.UpdateUserPassword;
using Application.User.Application.Query.GetById;
using Application.User.Application.Query.GetCount;
using Application.Users.Application.Comands.CreateUser;
using Application.Users.Application.Query.GetAll;
using UserServices;
using ApplicationUser.Application.Query.GetAll;
using Users.Application.Comands.DeleteUser;
using MediatR;
using Users.Application.Query.GetById;

namespace UserAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetListQuery query, 
            IMediator mediator,
            CancellationToken token)
        {
            var result = await mediator.Send(query, token);
            var count = await mediator.Send(new GetCountQuery() { Name = query.Name }, token);
            HttpContext.Response.Headers.Append("X-Total-Count", count.ToString());
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, IMediator mediator, CancellationToken token)
        {
            var item = await mediator.Send(new GetByIdQuery { Id = id }, token);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUserComand item, IMediator mediator, CancellationToken cancellationToken)
        {
            var postedItem = await mediator.Send(item, cancellationToken);
            return Created($"/users/{postedItem.Id}", postedItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateUserComand newItem, IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(newItem, cancellationToken);
            return Ok(item);
        }

        [HttpPut("ResetPassword ")]
        public async Task<IActionResult> ResetPassword(ResetPasswordComand newItem, IMediator mediator, CancellationToken cancellationToken)
        {
            var item = await mediator.Send(newItem, cancellationToken);
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, IMediator mediator)
        {
            return Ok(await mediator.Send(new DeleteUserComand() { Id = id }));
        }
    }
}
