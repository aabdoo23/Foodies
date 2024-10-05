using System.Security.Claims;
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
            var restaurant = await _context.Restaurant
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            var menuItems = await _context.MenuItem
                .Where(m => m.Resturant.Id == restaurantId && (category == null || m.Category == category))
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

        public async Task<IActionResult> LoadMenuItems(int restaurantId, string? category = null)
        {
            var menuItems = await _context.MenuItem
                .Where(m => m.Resturant.Id == restaurantId && (category == null || m.Category == category))
                .ToListAsync();

            return PartialView("_MenuItems", menuItems);
        }

        public async Task<IActionResult> Rate(int restaurantId)
        {
            var restaurant = await _context.Restaurant.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            var model = new RatingViewModel
            {
                RestaurantId = restaurantId,
                RestaurantName = restaurant.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var existingRating = await _context.Rating
                    .FirstOrDefaultAsync(r => r.RestaurantId == model.RestaurantId && r.CustomerId == customerId);

                if (existingRating != null)
                {
                    existingRating.Rate = model.Rate;
                    _context.Rating.Update(existingRating);
                }
                else
                {
                    var rating = new Rating
                    {
                        RestaurantId = model.RestaurantId,
                        Rate = model.Rate,
                        CustomerId = customerId,
                    };
                    await _context.Rating.AddAsync(rating);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Invalid rating data.");
        }
    }
}


