using Microsoft.AspNetCore.Mvc;
using Product.API.DTOs;
using Product.API.Interfaces;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBlobService _imageService;
        public ProductController(IProductService productService, IBlobService imageService)
        {
            _productService = productService;
            _imageService = imageService;
        }

        [HttpGet("hello")]
        public ActionResult Hello()
        {
            return Ok("HEllo");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var result = await _productService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await _productService.GetAllAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]AddProductDto productDto)
        {
            try
            {
                await _productService.AddAsync(productDto);
                return Ok(new { message = "Created" });
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
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("image")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var result = await _imageService.UploadAsync(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}
