using Foodies.Interfaces.Repositories;
using Foodies.Interfaces.Services;
using Foodies.Models;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace Foodies.Controllers
{
    public class MasterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ICustomerService _customerService;
        private readonly IAdminService _adminService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ImageUploader _imageUploader;

        public MasterController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICustomerService customerService,
            IAdminService adminService,
            IRestaurantRepository repository,
            ImageUploader imageUploader)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
            _adminService = adminService;
            _restaurantRepository = repository;
            _imageUploader = imageUploader;
        }

        public IActionResult view()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserSignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewCustomer(RegistrationViewModel cus)
        {
            if (ModelState.IsValid)
            {
                //TODO: Edit function to handle img
                var result = await _customerService.CreateCustomer(cus);
                if (result != null)
                {
                    await _signInManager.SignInAsync(result.IdentityUser, isPersistent: false);
                    ViewBag.NotificationMessage = "Customer registered successfully!";
                    ViewBag.NotificationType = "success";
                    return RedirectToAction("restaurant", "menu", result.Id);
                }
                else
                {
                    ViewBag.NotificationType = "danger";
                    return View("UserSignUp");
                }
            }
            else
            {
                ViewBag.NotificationMessage = "There are missing data.";
                ViewBag.NotificationType = "danger";
                return View("UserSignUp");
            }
        }

        public IActionResult AdminSignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdminAndResturant(AdminRegisterViewModel admin) // TODO: Add file upload again after fixing the image uploader
        {
            if (ModelState.IsValid)
            {
                Restaurant res = new Restaurant
                {
                    Name = admin.Name,
                    Photo = admin.Photo,
                    Hotline = admin.Hotline,
                    CuisineType = admin.CuisineType,
                    MaxPrice = admin.MaxPrice,
                    MinPrice = admin.MinPrice
                };
                var restaurantResult = await _restaurantRepository.Create(res);
                if (restaurantResult != null)
                {
                    var result = await _adminService.CreateAdmin(admin, restaurantResult);
                    if (result != null)
                    {
                        await _signInManager.SignInAsync(result.IdentityUser, isPersistent: false);

                        res.RestaurantAdmin = result;
                        await _restaurantRepository.Update(res);

                        ViewBag.NotificationMessage = "Admin registered successfully!";
                        ViewBag.NotificationType = "success";
                        return RedirectToAction("AdminProfile", "Home", new { id = result.Id });
                    }
                    else
                    {
                        ViewBag.NotificationType = "danger";
                    }

                }
                else
                {
                    ViewBag.NotificationMessage = "Restaurant registration failed.";
                    ViewBag.NotificationType = "danger";
                }

            }
            else
            {
                ViewBag.NotificationMessage = "There are missing data.";
                ViewBag.NotificationType = "danger";
            }

            return View("AdminSignUp", admin);

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel loginUser)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    string x = string.Join(", ", roles);

                    if (x == "Customer")
                    {
                        return RedirectToAction("restaurant", "menu");
                    }
                    else if (x == "Admin")
                    {
                        return RedirectToAction("AdminProfile", "Home", new { id = user.Id });
                    }
                    else
                    {
                        return RedirectToAction("Profile", "BranchManager", new { id = user.Id });
                    }
                }
                else
                {
                    ViewBag.NotificationMessage = "Unrecognized role.";
                    ViewBag.NotificationType = "danger";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.NotificationMessage = "Login failed. Incorrect password.";
                ViewBag.NotificationType = "danger";
                return View("Login");
            }
            
            
        }
        public async Task<IActionResult> Logout()
        {
            // Call the SignOutAsync method to log the user out
            await _signInManager.SignOutAsync();

            // Optionally, you can redirect the user to a different page after logout
            return RedirectToAction("view", "Master");
        }

    }
}