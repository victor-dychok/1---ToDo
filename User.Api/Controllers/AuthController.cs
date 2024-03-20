using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserServices;
using UserServices.dto;

namespace UserAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAutnService _authService;
        private readonly IUserService _userService;
        public AuthController(IAutnService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("CreateJwtToken")]
        public async Task<IActionResult> Post(AuthDto authDto)
        {
            var token = await _authService.GetJwtTokenAsync(authDto);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("RefreshJwtToken")]
        public async Task<IActionResult> Post(string jwtToken, CancellationToken cancellation)
        {
            var token = await _authService.GetJwtByRefreshTokenAsync(jwtToken, cancellation);
            return Ok(token);
        }

        [HttpGet("GetMyInfo")]
        public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
        {
            var currentId = User.FindFirst(ClaimTypes.NameIdentifier)!;
            var user = await _userService.GetByIdOrDefaultAsync(int.Parse(currentId.Value), cancellationToken);
            return Ok(user);
        }
    }
}
