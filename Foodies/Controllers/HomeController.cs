using System.Diagnostics;
using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Foodies.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodiesDbContext context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FoodiesDbContext context, ILogger<HomeController> logger)
        {
            this.context = context;
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
            var restaurant = await context.Restaurant.Include(x=>x.MenuItems)
                .SingleOrDefaultAsync(x => x.Id == adm.RestaurantId);
            ViewBag.Rest = restaurant;  // Assign restaurant to ViewBag.Rest
         

            ViewBag.menu =context.MenuItem.Where(x => x.Resturant.Id== adm.RestaurantId).ToList();
            return View(adm);
        }
        public IActionResult Addmnuitm(int id)
        {
            // Get the restaurant by id
            var restu = context.Restaurant.FirstOrDefault(x => x.Id == id);
          
            return View(restu); // Pass the restaurant to the view
        }

        public IActionResult SaveEditinmnu(MenuItem Menu, int restaurantId) {
            MenuItem mnu = new MenuItem();
            mnu.Name = mnu.Name;
            mnu.Category = Menu.Category;
            mnu.Name = Menu.Name;
            mnu.Description = Menu.Description;
            var Rest = context.Restaurant.FirstOrDefault(x => x.Id == restaurantId);
            mnu.Resturant = Rest;
            context.Add(mnu);
            context.SaveChanges();
            var adm = context.Admin.SingleOrDefault(x => x.RestaurantId == Rest.Id);
            return RedirectToAction("AdminProfile", adm);

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
        public IActionResult AddMenuItem(MenuItem Menu)
        {
            MenuItem mnu = new MenuItem();
            mnu.Category = Menu.Category;
            mnu.Name = Menu.Name;
            mnu.Description = Menu.Description;
            mnu.Resturant = Menu.Resturant;
            context.MenuItem.Add(Menu);
            context.SaveChanges();
            return RedirectToAction("AdminProfile");

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
