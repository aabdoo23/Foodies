using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
