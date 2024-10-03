using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class MasterController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MasterController(FoodiesDbContext context, 
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult view()
        {
            return View();
        }
        public IActionResult Ressolginsignup()
        {
            return View();
        }
        public IActionResult Cusolginsignup()
        {
            return View();
        }
        public IActionResult UserSignUp()
        {
            return View();
        }
        public async Task<IActionResult> SaveNewCustomer(RegisterationViewModel cus)
        {
            
            if (ModelState.IsValid)
            {

                var existingCustomer = await _userManager.FindByEmailAsync(cus.Email);
                if (existingCustomer == null)
                {
                    //fill identity info
                    IdentityUser user = new IdentityUser();
                    user.UserName = cus.Email;
                    user.PasswordHash = cus.Password;
                    user.Email = cus.Email;
                    user.PhoneNumber = cus.phoneNumber;


                    var result = await _userManager.CreateAsync(user, cus.Password);

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

                    //customer.Id = user.Id;
                    //cus.Address.Customer = customer;

                    _context.Customer.Add(customer);
                    _context.SaveChanges();

                    if (result.Succeeded)
                    {
                        ViewBag.NotificationMessage = "Customer registered successfully!";
                        ViewBag.NotificationType = "success";
                        return RedirectToAction("Cusolginsignup");
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

            return View( cus);
        }

        public IActionResult AdminSignUp()
        {
            return View();
        }
        //public async Task<IActionResult> SaveAdminAndResturant(Restaurant res, Admin adm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var resturannam = await _context.Restaurant.FirstOrDefaultAsync(x => x.Name == res.Name);
        //        var Admininsystim = await _userManager.FindByEmailAsync(adm.Email);
        //        if (resturannam == null && Admininsystim == null)
        //        {

        //            _context.Restaurant.Add(res);
        //            _context.SaveChanges();

        //            var rres = _context.Restaurant.Where(x => x.Name == res.Name).FirstOrDefault();

        //            adm.RestaurantId = rres.Id;
        //            adm.Restaurant = rres;

        //            await _userManager.CreateAsync(adm);
        //            return RedirectToAction("ResturantLogIn", "Master");
        //        }
        //        else if (resturannam != null && Admininsystim != null)
        //        {
        //            ViewBag.NotificationMessage = "The Resturant Name and Email already in the system";
        //            ViewBag.NotificationType = "danger";
        //        }
        //        else if (resturannam != null)
        //        {
        //            ViewBag.NotificationMessage = "The Resturant Name already in the system";
        //            ViewBag.NotificationType = "danger";
        //            //return View("UserSignUp", cus);
        //        }
        //        else
        //        {
        //            ViewBag.NotificationMessage = "The Email already in the system";
        //            ViewBag.NotificationType = "danger";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.NotificationMessage = "There are missing data.";
        //        ViewBag.NotificationType = "danger";
        //    }

        //    return View("AdminSignUp");
        //}
        public IActionResult CustomerLogIn()
        {
            return View();
        }
        //public async Task<IActionResult> ConfirmCustomerLogIn(string email, string pass)
        //{
        //    var existingCustomer = await _userManager.FindByEmailAsync(email);
        //    if (existingCustomer != null)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(existingCustomer, pass, false, false);
        //        if (!result.Succeeded)
        //        {
        //            ViewBag.NotificationMessage = "Wrong Email or Password";
        //            ViewBag.NotificationType = "danger";
        //            return View("CustomerLogIn");
        //        }
        //        return RedirectToAction("index", "CustomerView", existingCustomer);
        //    }
        //    else
        //    {
        //        ViewBag.NotificationMessage = "Wrong Email or Password";
        //        ViewBag.NotificationType = "danger";
        //        return View("CustomerLogIn");
        //    }
        //}
        public IActionResult ResturantLogIn()
        {
            return View();
        }
        //public async Task<IActionResult> REsturantonerLogIn(string email, string pass)
        //{
        //    var existingAdmin = await _userManager.FindByEmailAsync(email);
        //    if (existingAdmin != null)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(existingAdmin, pass, false, false);
        //        if (!result.Succeeded)
        //        {
        //            ViewBag.NotificationMessage = "wrong email or password";
        //            ViewBag.NotificationType = "danger";
        //            return View("ResturantLogIn");
        //        }

        //        return RedirectToAction("AdminProfile", "Home", existingAdmin);
        //    }
        //    else
        //    {
        //        ViewBag.NotificationMessage = "wrong email or password";
        //        ViewBag.NotificationType = "danger";
        //        return View("ResturantLogIn");
        //    }

        //}

    }
}



