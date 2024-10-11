using Foodies.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Foodies.Controllers
{
    public class CustomerViewController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        public CustomerViewController(
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository)
        {
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<IActionResult> index(Customer Cus)
        {
            var mnui = ViewData["Reslist"] = await _restaurantRepository.GetAll();
            //	Console.WriteLine(
            //Console.WriteLine(Cus);
            return View(Cus);
        }
        public async Task<IActionResult> Menu(string id)
        {
            var rest = await _restaurantRepository.GetById(id);
            ViewData["data"] = await _menuItemRepository.GetAllByRestaurantId(id);
            // var result=context.MenuItems.Where(x=>x.Resturant.RestaurantId==id).ToList();
            return View(rest);

        }
    }
}

