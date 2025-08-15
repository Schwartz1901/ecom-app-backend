using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using User.API.DTOs;
using User.API.Interfaces;
using System.Security.Claims;
namespace User.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("hello")]
        
        public ActionResult Hello()
        {
            return Ok("Hello From User");
        }
        //[Authorize]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById([FromRoute] Guid id)
        //{
        //    var aid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var username = User.FindFirst("username")?.Value;
        //    var email = User.FindFirst(ClaimTypes.Email)?.Value;
        //    try
        //    {
        //        var user = await _userService.GetByIdAsync(id, aid, username, email);
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {

            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                return Unauthorized();
            }
            try
            {

                var profile = await _userService.GetByAuthIdAsync(id);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            try
            {
                var result = await _userService.CreateUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {source="User service", message = ex.Message })
;            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
