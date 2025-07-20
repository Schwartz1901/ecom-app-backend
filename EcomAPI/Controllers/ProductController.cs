using EcomAPI.Controllers.Dtos;
using EcomAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcomAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController (IProductService productService) 
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
   
            var products = await _productService.GetProductsAsync();
            return Ok(products);
 
           
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
            
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            
            var createdProduct = await _productService.AddProductAsync(productDto);
            return CreatedAtAction(
                nameof(GetProduct),          
                new { id = createdProduct.Id },  
                createdProduct             
            );
             
            
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductDto productDto)
        {
            var product = await _productService.UpdateProductAsync(id, productDto);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var isDelete = await _productService.DeleteProductAsync(id);
            return Ok(isDelete);
        }
    }
}
