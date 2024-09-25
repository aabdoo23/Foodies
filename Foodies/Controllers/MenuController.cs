using Foodies.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class MenuController : Controller
    {
        private readonly FoodiesDbContext _context;

        public MenuController(FoodiesDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int restaurantId, string? category = null)
        {
            var restaurant = await _context.Restaurant.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            var menuItems = await _context.MenuItem
                .Where(m => m.Resturant.Id == restaurantId)
                .ToListAsync();

            var categories = menuItems
                .Select(m => m.Category)
                .Distinct()
                .ToList();

            var viewModel = new MenuViewModel
            {
                Restaurant = restaurant,
                MenuItems = menuItems,
                SelectedCategory = category,
                Categories = categories
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Restaurant()
        {
            var restaurants = await _context.Restaurant.ToListAsync();
            return View(restaurants);
        }
    }
}