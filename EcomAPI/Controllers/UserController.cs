using EcomAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetUser()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserAsync(Guid.Parse(userId));
            return Ok(user);
          
        }
    }
}
