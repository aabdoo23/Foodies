using Foodies.Interfaces.Repositories;
using Foodies.Interfaces.Services;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace Foodies.Controllers
{
    public class MasterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICustomerService _customerService;
        private readonly IAdminService _adminService;
        private readonly IRestaurantRepository _restaurantRepository;

        private readonly SignInManager<IdentityUser> _signInManager;

        public MasterController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICustomerService customerService,
            IAdminService adminService,
            IRestaurantRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerService = customerService;
            _adminService = adminService;
            _restaurantRepository = repository;
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
                var result = await _customerService.CreateCustomer(cus);
                if (result != null)
                {
                    await _signInManager.SignInAsync(result.IdentityUser, isPersistent: false);
                    ViewBag.NotificationMessage = "Customer registered successfully!";
                    ViewBag.NotificationType = "success";
                }
                else
                {
                    ViewBag.NotificationType = "danger";
                }
            }
            else
            {
                ViewBag.NotificationMessage = "There are missing data.";
                ViewBag.NotificationType = "danger";
            }
            return View("UserSignUp");
        }

        public IActionResult SaveAdminAndResturant()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdminAndResturant(AdminRegisterViewModel admin)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.CreateAdmin(admin);
                if (result != null)
                {
                    await _signInManager.SignInAsync(result.IdentityUser, isPersistent: false);
                    Restaurant res = new Restaurant
                    {
                        Name = admin.Name,
                        Photo = admin.Photo,
                        Hotline = admin.Hotline,
                        CuisineType = admin.CuisineType,
                        MaxPrice = admin.MaxPrice,
                        MinPrice = admin.MinPrice,
                    };
                    res.RestaurantAdmin = result;
                    await _restaurantRepository.Create(res);
                    ViewBag.NotificationMessage = "Customer registered successfully!";
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
                ViewBag.NotificationMessage = "There are missing data.";
                ViewBag.NotificationType = "danger";
            }

            return View(admin);
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
                        return RedirectToAction("UserView", "Home", new { id = user.Id });
                    }
                    else if (x == "Admin")
                    {
                        return RedirectToAction("AdminProfile", "Home", new { id = user.Id });
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
            else
            {
                ViewBag.NotificationMessage = "User not found.";
                ViewBag.NotificationType = "danger";
                return View("Login");
            }

        }
    }
}

