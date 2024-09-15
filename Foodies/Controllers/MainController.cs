using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
