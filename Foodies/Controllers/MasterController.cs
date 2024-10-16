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
        public async Task<IActionResult> SaveNewCustomer(RegistrationViewModel cus, IFormFile immg)
        {
            if (ModelState.IsValid)
            {
                string? usrl = await _imageUploader.UploadImageAsync(immg);
                cus.img = usrl;
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
        public async Task<IActionResult> SaveAdminAndResturant(AdminRegisterViewModel admin, IFormFile immg) 
        {
            string? usrl = await _imageUploader.UploadImageAsync(immg);
            if (ModelState.IsValid)
            {
                Restaurant res = new Restaurant
                {
                    
                    Name = admin.Name,
                    Photo = usrl,
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
        public async Task<IActionResult> AboutUs()
        {
            return view();
        }
        public async Task<IActionResult> Main()
        {
            var userid =  _userManager.GetUserId(User);
            // Call the SignOutAsync method to log the user out
            if (User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("Customer")){
                    return RedirectToAction("restaurant", "menu");
                }
                else if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminProfile", "Home", new { id = userid });
                }
                else
                {
                    return RedirectToAction("Profile","BranchManager");
                }
            }
            else
            {
                // Redirect to the default view
                return RedirectToAction("view", "Master");
            }
        }
    }
}