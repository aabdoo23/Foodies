using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foodies.Controllers
{
    public class MenuAdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        private readonly FoodiesDbContext _context;
        private readonly UserManager<Admin> _userManager;

        public MenuAdminController(FoodiesDbContext context, UserManager<Admin> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // List Menu Items for the Logged-in Admin's Restaurant
        public async Task<IActionResult> Admin()
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in admin
            var restaurant = await _context.Restaurant
                .Include(r => r.MenuItems) // Include the restaurant's menu items
                .FirstOrDefaultAsync(r => r.RestaurantAdmin.AdminId == user.AdminId);

            if (restaurant == null)
            {
                return Unauthorized(); // Make sure the user is authorized to view their restaurant
            }

            return View(restaurant.MenuItems);
        }

        // Display form to add a new menu item
        public IActionResult Create()
        {
            return View();
        }

        // Handle form submission to add a new menu item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            var user = await _userManager.GetUserAsync(User);
            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(r => r.RestaurantAdmin.AdminId == user.AdminId);

            if (restaurant == null)
            {
                return Unauthorized();
            }

            menuItem.Resturant = restaurant; // Set the restaurant for the menu item
            _context.MenuItem.Add(menuItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit Menu Item
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _context.MenuItem.Include(m => m.Resturant)
                .FirstOrDefaultAsync(m => m.Id == id);

            var user = await _userManager.GetUserAsync(User);
            if (menuItem == null || menuItem.Resturant.RestaurantAdmin.AdminId != user.AdminId)
            {
                return Unauthorized();
            }

            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItem menuItem)
        {
            var user = await _userManager.GetUserAsync(User);
            var restaurant = await _context.Restaurant
                .FirstOrDefaultAsync(r => r.RestaurantAdmin.AdminId == user.AdminId);

            if (restaurant == null || menuItem.Resturant.Id != restaurant.Id)
            {
                return Unauthorized();
            }

            _context.Update(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Delete Menu Item
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _context.MenuItem.Include(m => m.Resturant)
                .FirstOrDefaultAsync(m => m.Id == id);

            var user = await _userManager.GetUserAsync(User);
            if (menuItem == null || menuItem.Resturant.RestaurantAdmin.AdminId != user.AdminId)
            {
                return Unauthorized();
            }

            return View(menuItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItem.Include(m => m.Resturant)
                .FirstOrDefaultAsync(m => m.Id == id);

            var user = await _userManager.GetUserAsync(User);
            if (menuItem == null || menuItem.Resturant.RestaurantAdmin.AdminId != user.AdminId)
            {
                return Unauthorized();
            }

            _context.MenuItem.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

