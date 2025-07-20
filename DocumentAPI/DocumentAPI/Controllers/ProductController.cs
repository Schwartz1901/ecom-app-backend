using DocumentAPI.Controllers.DTOs;
using DocumentAPI.Interfaces;
using DocumentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok("Hello");
        }
        [HttpGet]
        // GET api/Product/
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        // GET api/Product/{id}
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            
        }
        [HttpGet("search")]
        // GET api/Product/search?name={name}
        public async Task<IActionResult> Search([FromQuery] string name)
        {
        
            var products = await _productService.SearchAsync(name);
            return Ok(products);
        }
        [HttpPost]
        // POST api/Product 
        public async Task<IActionResult> Create([FromBody] ProductDto product) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState)
            var created = await _productService.AddAsync(product);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductDto productDto) // Deserialize json to object
        {
            
            var product = await _productService.UpdateAsync(id, productDto);
            if (product == null)
            {
                return NotFound("Cannot find product with Id " + id);
            }
            // Serialize object to json
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Id");
            }
            var isDelete = await _productService.DeleteAsync(id);
            if (!isDelete)
            {
                return NotFound("Cannot find product with Id " + id);
            }
            return Ok("Product deleted.");
        }
    }
}
