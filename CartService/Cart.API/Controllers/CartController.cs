using Cart.API.DTOs;
using Cart.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.CartControllers { 

    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) {
            _cartService = cartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            try
            {
                var result = await _cartService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCartRequestDto request)
        {
            try
            {
                var result = await _cartService.CreateAsync(request.UserId);
                return CreatedAtAction(nameof(GetById), new { id = result }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {source= "Cart service", message = ex.Message });
            }
        }
        
        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDto dto)
        {
            try
            {
                var authId = User.FindFirst("sub")?.Value;
                if (authId == null) {
                    return Unauthorized();
                }
                await _cartService.AddItemToCartAsync(authId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           try
            {
                await _cartService.DeleteAsync(id);
                return Ok("Deleted");
            }

        catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
