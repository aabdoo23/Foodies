using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
	public class CustomerViewController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
