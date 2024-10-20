﻿using System.Security.Claims;
using Foodies.Interfaces.Repositories;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class MenuController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public MenuController(
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,
            IRatingRepository ratingRepository,
            ICustomerRepository customerRepository,
            UserManager<IdentityUser> userManager)
        {
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
            _ratingRepository = ratingRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string restaurantId, string? category = null)
        {
            var restaurant = await _restaurantRepository.GetByIdWithRatings(restaurantId);

            if (restaurant == null) return NotFound();
            Response.Cookies.Append("r", restaurantId);

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
        [HttpPost]
        public async Task<IActionResult> addToFav(string resId)
        {

            var userId = _userManager.GetUserId(User);
            Restaurant res = await _restaurantRepository.GetByIdWithFavouriteCustomers(resId);

            Customer customer = await _customerRepository.GetByIdWithFavouriteRestaurants(userId);


            if (!customer.FavouriteRestaurants.Contains(res) && !res.FavouriteCustomers.Contains(customer))
            {
                res.FavouriteCustomers.Add(customer);
                customer.FavouriteRestaurants.Add(res);
            }
            await _customerRepository.Update(customer);
            await _restaurantRepository.Update(res);
            return Content($"added to fav -{customer.Id}{res.Name}");
        }
        [HttpPost]
        public async Task<IActionResult> removeFav(string resId)
        {

            var userId = _userManager.GetUserId(User);
            Restaurant res = await _restaurantRepository.GetByIdWithFavouriteCustomers(resId);

            Customer customer = await _customerRepository.GetByIdWithFavouriteRestaurants(userId);

            if (customer.FavouriteRestaurants.Contains(res) && res.FavouriteCustomers.Contains(customer))
            {
                res.FavouriteCustomers.Remove(customer);
                customer.FavouriteRestaurants.Remove(res);
            }

            await _customerRepository.Update(customer);
            await _restaurantRepository.Update(res);
            return Content($"rem fav -{customer.Id}{res.Name}");
        }
        public async Task<IActionResult> Favourite()
        {
            var userId = _userManager.GetUserId(User);
            Customer customer = await _customerRepository.GetByIdWithFavouriteRestaurants(userId);


            return View(customer.FavouriteRestaurants);
        }
        public async Task<IActionResult> Restaurant(string id)
        {
            ViewBag.cusid = id;
            var restaurants = await _restaurantRepository.GetAll();

            var userId = _userManager.GetUserId(User);

            Customer customer = await _customerRepository.GetByIdWithFavouriteRestaurants(userId);

            ViewBag.fav = customer;

            //return Content($"{ViewBag.fav.FavouriteRestaurants}");
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
            try
            {
                if (ModelState.IsValid)
                {
                    var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var existingRating = await _ratingRepository.GetByCustomerIdAndRestaurantId(customerId, model.RestaurantId);

                    if (existingRating != null)
                    {
                        existingRating.Rate = model.Rate;
                        await _ratingRepository.Update(existingRating);
                        return Ok("Rating updated successfully.");
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
                        return Ok("Rating created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding/updating rating: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }

            return BadRequest("Invalid rating data.");
        }

    }
}


