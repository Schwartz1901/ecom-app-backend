using Microsoft.AspNetCore.Mvc;

namespace EcomAPI.Controllers
{

    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok();
        }
    }
}
