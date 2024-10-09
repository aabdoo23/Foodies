using Foodies.Common;
using Foodies.Data;
using Foodies.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
namespace Foodies.Controllers
{
    public class CustomerViewController : Controller
    {
        private readonly IBaseRepository<Restaurant> _restaurantRepository;
        //private readonly IBaseRepository<MenuItem> _menuItemRepository;
        private readonly MenuItemRepository _menuItemRepository;
        public CustomerViewController(
            IBaseRepository<Restaurant> restaurantRepository,
            MenuItemRepository menuItemRepository)
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

