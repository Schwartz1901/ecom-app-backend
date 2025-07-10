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
        [HttpPost]
        // POST api/Product 
        public async Task<IActionResult> Create(ProductEntity product)
        {
            await _productService.AddAsync(product);
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }
    }
}
