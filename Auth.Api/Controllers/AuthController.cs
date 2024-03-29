using Auth.Api.dto;
using Auth.Api.Services;
using Auth.Application.Comands.CreateJwtToken;
using Auth.Application.Comands.RefreshJwtToken;
using Auth.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("CreateJwtToken")]
        public async Task<IActionResult> Post(CreateJwtTokenComand comand, IMediator mediator, CancellationToken cancellation)
        {
            var token = await mediator.Send(comand, cancellation);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("RefreshJwtToken")]
        public async Task<IActionResult> Post(string jwtToken, IMediator mediator, CancellationToken cancellation)
        {
            var token = await mediator.Send(new RefreshJwtTokenComand() { RefreshToken = jwtToken }, cancellation);
            return Ok(token);
        }
        
        [HttpGet("GetMyInfo")]
        public async Task<IActionResult> GetMyInfo(IMediator mediator, CancellationToken cancellationToken)
        {
            var currentId = User.FindFirst(ClaimTypes.NameIdentifier)!;
            var user = await mediator.Send(new GetByIdQuery() { Id = int.Parse(currentId.Value) }, cancellationToken);
            return Ok(user);
        }
    }
}
