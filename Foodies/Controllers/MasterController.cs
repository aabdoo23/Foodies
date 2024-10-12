using Foodies.Models;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace Foodies.Controllers
{
    public class MasterController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<IdentityUser> _signInManager;

       private readonly ImageUploader _imageUploader;

        public MasterController(FoodiesDbContext context,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ImageUploader imageUploader)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _imageUploader = imageUploader;
        }

        public async Task CreateRole()
        {

            //if (!_roleManager.RoleExistsAsync(UserRoles.Customer).GetAwaiter().GetResult())
            //{
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.BranchManager)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin)).GetAwaiter().GetResult();
            //}
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
        public async Task<IActionResult> SaveNewCustomer(RegisterationViewModel cus, IFormFile immg)
        {

            if (ModelState.IsValid)
            {

                var existingCustomer = await _userManager.FindByEmailAsync(cus.Email);
                if (existingCustomer == null)
                {
                    //fill identity info
                    CreateRole();
                    IdentityUser user = new IdentityUser();
                    user.UserName = cus.Email;
                    user.Email = cus.Email;
                    user.PhoneNumber = cus.phoneNumber;

                    string? usrl = await _imageUploader.UploadImageAsync(immg);
                    IdentityResult result = await _userManager.CreateAsync(user, cus.Password);

                    Customer customer = new Customer
                    {
                        Id = user.Id,
                        FirstName = cus.FirstName,
                        LastName = cus.LastName,
                        img = usrl,

                        Address = new Address // Initialize Address object
                        {
                            City = cus.City,
                            Street = cus.Street,
                            Building = cus.Building,
                            Location = cus.Location,
                        },
                        IdentityUser = user,

                    };

                    _context.Customer.Add(customer);
                    _context.SaveChanges();

                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(user, "Customer");
                        Customer cust = await _context.Customer
                        .Include(c => c.FavouriteRestaurants)
                        .FirstOrDefaultAsync(x => x.Id == user.Id);

                        ViewBag.fav = cust;
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        ViewBag.NotificationMessage = "Customer registered successfully!";
                        ViewBag.NotificationType = "success";
                        //return RedirectToAction("Cusolginsignup");
                        return RedirectToAction("restaurant", "menu", customer.Id);


                    }
                    else
                    {
                        ViewBag.NotificationMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                        ViewBag.NotificationType = "danger";
                        return View("UserSignUp");

                    }
                }
                else
                {
                    ViewBag.NotificationMessage = "The email is already registered.";
                    ViewBag.NotificationType = "danger";
                    return View("UserSignUp", cus);

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
        public async Task<IActionResult> SaveAdminAndResturant(AdminRegisterViewModel admin,IFormFile immg)
        {
            if (ModelState.IsValid)
            {

                var existingCustomer = await _userManager.FindByEmailAsync(admin.Email);
                if (existingCustomer == null)
                {
                    //fill identity info
                    CreateRole();

                    IdentityUser user = new IdentityUser();
                    user.UserName = admin.Email;
                    user.Email = admin.Email;
                    user.PhoneNumber = admin.phoneNumber;
                    string? usrl =await _imageUploader.UploadImageAsync(immg);

                    var result = await _userManager.CreateAsync(user, admin.Password);

                    Admin adminn = new Admin
                    {
                        Id = user.Id,
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        IdentityUser = user,

                    };
                    Restaurant res = new Restaurant
                    {
                        Name = admin.Name,
                        Photo = usrl,
                        Hotline = admin.Hotline,
                        CuisineType = admin.CuisineType,
                        MaxPrice = admin.MaxPrice,
                        MinPrice = admin.MinPrice,
                      
              

                    };
                    //custo}mer.Id = user.Id;
                    //cus.Address.Customer = customer;
                    res.RestaurantAdmin = adminn;
                    _context.Admin.Add(adminn);
                    _context.Restaurant.Add(res);

                    _context.SaveChanges();

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");

                        await _signInManager.SignInAsync(user, isPersistent: false);


                        ViewBag.NotificationMessage = "Customer registered successfully!";
                        ViewBag.NotificationType = "success";
                        return RedirectToAction("AdminProfile", "Home", new { id = user.Id });
                    }
                    else
                    {
                        ViewBag.NotificationMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                        ViewBag.NotificationType = "danger";
                    }
                }
                else
                {
                    ViewBag.NotificationMessage = "The email is already registered.";
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

                    //        /menu/restaurant
                    if (x == "Customer")
                    {
                        //var cus = _context.Customer.Where(x => x.Id == user.Id);
                        Customer cus = await _context.Customer
                        .Include(c => c.FavouriteRestaurants)
                        .FirstOrDefaultAsync(x => x.Id == user.Id);

                        ViewBag.fav = cus;
                        var cust = _context.Customer.Where(x => x.Id == user.Id);

                        return RedirectToAction("restaurant", "menu");
                    }
                    else if (x == "Admin")
                    {
                        return RedirectToAction("AdminProfile", "Home", new { id = user.Id }); //noooooooooooooo
                    }
                    else
                    {
                        return RedirectToAction("Profile", "BranchManager", new { id = user.Id });
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


        /*
            public IActionResult ResturantLogIn()
            {
                return View("login");
            }
            [HttpPost]
            public async Task<IActionResult> ResturantLogIn(LogInViewModel loginUser)
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = await _userManager.FindByEmailAsync(loginUser.Email);

                    if (user != null)
                    {
                        SignInResult result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);
                        if (result.Succeeded)
                        {

                            var x = _userManager.GetRolesAsync(user);
                            ViewBag.Roles = x;
                            return Content(ViewBag.Roles);
                        }
                        else
                        {
                            return Content("not success");
                        }
                    }
                    else
                    {
                        return Content("user null");
                    }
                }
                else
                {
                    return Content("state no");

                }

            }

        }
        */
    }
}