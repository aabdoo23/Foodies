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

        public MasterController(FoodiesDbContext context,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.BranchManager));
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
        }

        public IActionResult view()
        {
            return View();
        }

        public IActionResult SaveNewCustomer()
        {
            return View();
        }
        public IActionResult UserSignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewCustomer(RegisterationViewModel cus)
        {

            if (ModelState.IsValid)
            {

                var existingCustomer = await _userManager.FindByEmailAsync(cus.Email);
                if (existingCustomer == null)
                {
                    //fill identity info
                    await CreateRole();
                    IdentityUser user = new IdentityUser();
                    user.UserName = cus.Email;
                    user.Email = cus.Email;
                    user.PhoneNumber = cus.phoneNumber;


                    IdentityResult result = await _userManager.CreateAsync(user, cus.Password);

                    Customer customer = new Customer
                    {
                        Id = user.Id,
                        FirstName = cus.FirstName,
                        LastName = cus.LastName,

                        Address = new Address // Initialize Address object
                        {
                            City = cus.City,
                        },
                        IdentityUser = user,


                    };

                    _context.Customer.Add(customer);
                    _context.SaveChanges();

                    if (result.Succeeded)
                    {
                        //await _userManager.AddToRoleAsync(user, "Customer");

                        await _userManager.AddToRoleAsync(user, "Customer");

                        await _signInManager.SignInAsync(user, isPersistent: false);

                        ViewBag.NotificationMessage = "Customer registered successfully!";
                        ViewBag.NotificationType = "success";
                        //return RedirectToAction("Cusolginsignup");
                        return Content("User registered");

                    }
                    else
                    {
                        ViewBag.NotificationMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                        ViewBag.NotificationType = "danger";
                        return Content("user not registered");

                    }
                }
                else
                {
                    ViewBag.NotificationMessage = "The email is already registered.";
                    ViewBag.NotificationType = "danger";
                    return Content("user is null");

                }
            }
            else
            {
                ViewBag.NotificationMessage = "There are missing data.";
                ViewBag.NotificationType = "danger";
                return Content("model  no ");

            }

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

                var existingCustomer = await _userManager.FindByEmailAsync(admin.Email);
                if (existingCustomer == null)
                {
                    //fill identity info
                    CreateRole();

                    IdentityUser user = new IdentityUser();
                    user.UserName = admin.Email;
                    user.Email = admin.Email;
                    user.PhoneNumber = admin.phoneNumber;


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
                        Photo = admin.Photo,
                        Hotline = admin.Hotline,
                        CuisineType = admin.CuisineType,
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
                        return Content("User registered");
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

            return View(admin);
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(loginUser.Email);

                if (user != null)
                {
                    SignInResult result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);
                    if (result.Succeeded)
                    {


                        return Content("success wee");
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


                        return Content("success wee");
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
}



