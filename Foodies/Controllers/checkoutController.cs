using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class checkoutController : Controller
    {//checkout/checkout
        public IActionResult checkout()
        {
            return View();
        }
    }
}
