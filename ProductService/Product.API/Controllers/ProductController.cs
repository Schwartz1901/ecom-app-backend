using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [HttpGet("hello")]
        public ActionResult Hello()
        {
            return Ok("HEllo");
        }
        
    }
}
