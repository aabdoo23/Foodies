using System.Security.Claims;
using Foodies.Data;
using Foodies.Interfaces.Repositories;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class MenuController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRatingRepository _ratingRepository;
        public MenuController(
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,
            IRatingRepository ratingRepository)
        {
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
            _ratingRepository = ratingRepository;
        }

        public async Task<IActionResult> Index(string restaurantId, string? category = null)
        {
            var restaurant = await _restaurantRepository.GetByIdWithRatings(restaurantId);

            if (restaurant == null)return NotFound();

            var menuItems = await _menuItemRepository.GetAllByRestaurantId(restaurantId, category);

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
            var restaurants = await _restaurantRepository.GetAll();
            return View(restaurants);
        }

        public async Task<IActionResult> LoadMenuItems(string restaurantId, string? category = null)
        {
            var menuItems = await _menuItemRepository.GetAllByRestaurantId(restaurantId, category);

            return PartialView("_MenuItems", menuItems);
        }

        public async Task<IActionResult> Rate(string restaurantId)
        {
            var restaurant = await _restaurantRepository.GetById(restaurantId);
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

                var existingRating = await _ratingRepository.GetByCustomerIdAndRestaurantId(customerId, model.RestaurantId);

                if (existingRating != null)
                {
                    existingRating.Rate = model.Rate;
                    await _ratingRepository.Update(existingRating);
                }
                else
                {
                    var rating = new Rating
                    {
                        RestaurantId = model.RestaurantId,
                        Rate = model.Rate,
                        CustomerId = customerId,
                    };
                    await _ratingRepository.Create(rating);
                }
                return Ok();
            }

            return BadRequest("Invalid rating data.");
        }
    }
}


