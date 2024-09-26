using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Foodies.Controllers
{
    public class HomeController : Controller
    {
        FoodiesDbContext context = new FoodiesDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult CustomerView()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> AdminProfile(Admin adm)
        {
            ViewBag.Rest =await context.Restaurant.Include(x=>x.MenuItems).SingleOrDefaultAsync(x => x.Id == adm.RestaurantId);
            ViewBag.menu =context.MenuItem.Where(x => x.Resturant.Id== adm.RestaurantId).ToList();
            return View(adm);
        }
        public IActionResult User(int id)
        {
            var Cus=context.Customer.SingleOrDefault(x => x.Id == id);
            return View(Cus);
        }
        public IActionResult AccountInfo(Customer Cus)
        {
            
            var Edit=context.Customer.SingleOrDefault(x=>x.Id == Cus.Id);
           // if (Cus.Email != Edit.Email)
          //  {
                var ch=context.Customer.SingleOrDefault(x=>x.Email == Cus.Email);
           //     if (ch == null) {
                    Edit.FirstName = Cus.FirstName;
                    Edit.LastName = Cus.LastName;
                    Edit.Email = Cus.Email;
                    Edit.PhoneNumber = Cus.PhoneNumber;
                    context.SaveChanges();
                    ViewBag.NotificationMessage = "Customer registered successfully!";
                    ViewBag.NotificationType = "success";
                    return RedirectToAction("User", Cus);
           //     }
          //  }
           // ViewBag.NotificationMessage = "The email is already registered.";
          //  ViewBag.NotificationType = "danger";
          //  return RedirectToAction("User",Cus);
        }

       public IActionResult CusAddress(Customer Cus)
        {
            var Edit = context.Customer.SingleOrDefault(x => x.Id == Cus.Id);
            Edit.City = Cus.City;
            Edit.Street = Cus.Street;
            Edit.Building = Cus.Building;

            context.SaveChanges();
            return RedirectToAction("User", Cus);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
