using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
