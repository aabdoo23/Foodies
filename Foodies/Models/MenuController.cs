using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
