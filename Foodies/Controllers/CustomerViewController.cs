using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Foodies.Controllers
{
    public class CustomerViewController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CustomerViewController(FoodiesDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult index(Customer Cus)
        {
            var mnui = ViewData["Reslist"] = _context.Restaurant.ToList();
            //	Console.WriteLine(
            //Console.WriteLine(Cus);
            return View(Cus);
        }
        public IActionResult Minew(int id)
        {
            var rest = _context.Restaurant.Where(x => x.Id == id).SingleOrDefault();
            ViewData["data"] = _context.MenuItem.Where(x => x.Resturant.Id == id).ToList();
            // var result=context.MenuItems.Where(x=>x.Resturant.RestaurantId==id).ToList();
            return View(rest);

        }
        public IActionResult Chat()
        {
            var customerId = _userManager.GetUserId(User);
            var signedInUser= _context.Admin.Where(x => x.IdentityUser.Id == customerId).SingleOrDefault();
            ChatViewModel viewModel = new ChatViewModel
            {

            };
            return View();
        }
    }
}

