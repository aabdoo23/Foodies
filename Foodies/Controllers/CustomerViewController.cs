using Microsoft.AspNetCore.Mvc;
namespace Foodies.Controllers
{
    public class CustomerViewController : Controller
    {
        private readonly FoodiesDbContext context;

        public CustomerViewController(FoodiesDbContext context)
        {
            this.context = context;
        }

        public IActionResult index(Customer Cus)
        {
            var mnui = ViewData["Reslist"] = context.Restaurant.ToList();
            //	Console.WriteLine(
            Console.WriteLine(Cus);
            return View(Cus);
        }
        public IActionResult Minew(int id)
        {

            var rest = context.Restaurant.Where(x => x.Id == id).SingleOrDefault();
            ViewData["data"] = context.MenuItem.Where(x => x.Resturant.Id == id).ToList();
            // var result=context.MenuItems.Where(x=>x.Resturant.RestaurantId==id).ToList();
            return View(rest);

        }




    }
}

