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
            //Validation
            if (id <= 0)
                return BadRequest("ID must be greater than 0");
            var product = await _productService.GetByIdAsync(id);
            
            return Ok(product);
        }
        [HttpGet("search")]
        // GET api/Product/search?name={name}
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
                return BadRequest("Search keyword must be at least 2 characters.");
            var products = await _productService.SearchAsync(name);
            return Ok(products);
        }
        [HttpPost]
        // POST api/Product 
        public async Task<IActionResult> Create([FromBody]ProductDto product)
        {
            if (product == null)
            {
                return BadRequest("Unknow product!");
            }
            var created = await _productService.AddAsync(product);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }
    }
}
