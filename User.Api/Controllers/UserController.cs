using Microsoft.AspNetCore.Mvc;
using UserServices;
using UserServices.dto;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;
        public UserController(IUserService users)
        {
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? offset, string? name, int? limit)
        {
            return Ok(await _users.GetListAsync(offset, name, limit));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _users.GetByIdOrDefaultAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDto item)
        {
            var postedItem = await _users.AddAsync(item);
            return Created($"/users/{postedItem.Id}", postedItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserDto newItem)
        {
            var item = await _users.UpdateAsync(newItem);
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _users.DeleteAsync(id));
        }
    }
}
