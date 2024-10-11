using System.Security.Claims;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodies.Controllers
{
    public class MenuController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MenuController(FoodiesDbContext context,
        UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]

        public async Task<IActionResult> addToFav(int resId)
        {
            
            var userId = _userManager.GetUserId(User);
            Restaurant res = await _context.Restaurant
               .Include(r => r.FavouriteCustomers)
               .FirstOrDefaultAsync(x => x.Id == resId);

            Customer customer = await _context.Customer
                .Include(c => c.FavouriteRestaurants)
                .FirstOrDefaultAsync(x => x.Id == userId);
            //res.FavouriteCustomers = new List<Customer>();
            //customer.FavouriteRestaurants = new List<Restaurant>();

            if (!customer.FavouriteRestaurants.Contains(res) && !res.FavouriteCustomers.Contains(customer)){
                res.FavouriteCustomers.Add(customer);

                customer.FavouriteRestaurants.Add(res);

                await _context.SaveChangesAsync();
            }

            return Content($"added to fav -{customer.Id}{res.Name}");
        }
        [HttpPost]
        public async Task<IActionResult> removeFav(int resId)
        {

            var userId = _userManager.GetUserId(User);
            Restaurant res = await _context.Restaurant
               .Include(r => r.FavouriteCustomers)
               .FirstOrDefaultAsync(x => x.Id == resId);

            Customer customer = await _context.Customer
                .Include(c => c.FavouriteRestaurants)
                .FirstOrDefaultAsync(x => x.Id == userId);
            //res.FavouriteCustomers = new List<Customer>();
            //customer.FavouriteRestaurants = new List<Restaurant>();

            if (customer.FavouriteRestaurants.Contains(res) && res.FavouriteCustomers.Contains(customer))
            {
                res.FavouriteCustomers.Remove(customer);

                customer.FavouriteRestaurants.Remove(res);

                await _context.SaveChangesAsync();
                return Content("weee");
                

            }


            return Content($"rem fav -{customer.Id}{res.Name}");
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

        public async Task<IActionResult> Favourite()
        {
            var userId = _userManager.GetUserId(User);
            Customer customer = await _context.Customer
                .Include(c => c.FavouriteRestaurants)
                .FirstOrDefaultAsync(x => x.Id == userId);

            ViewBag.fav = customer;


            return View();
        }
        public async Task<IActionResult> Restaurant(string id)
        {
            ViewBag.cusid=id;
            var restaurants = await _context.Restaurant.ToListAsync();
            var userId = _userManager.GetUserId(User);
            Customer customer = await _context.Customer
                .Include(c => c.FavouriteRestaurants)
                .FirstOrDefaultAsync(x => x.Id == userId);

            ViewBag.fav = customer;

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


