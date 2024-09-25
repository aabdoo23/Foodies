using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class OrderHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
