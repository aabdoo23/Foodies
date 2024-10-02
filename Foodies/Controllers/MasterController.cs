using Foodies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace Foodies.Controllers
{
    public class MasterController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly UserManager<Customer> _customerManager;
        private readonly UserManager<Admin> _adminManager;
        private readonly SignInManager<Customer> _customerSignInManager;
        private readonly SignInManager<Admin> _adminSignInManager;

        public MasterController(FoodiesDbContext _context, UserManager<Customer> customerManager, UserManager<Admin> adminManager, SignInManager<Admin> adminSignInManager, SignInManager<Customer> customerSignInManager)
        {
            this._context = _context;
            _customerManager = customerManager;
            _adminManager = adminManager;
            _adminSignInManager = adminSignInManager;
            _customerSignInManager = customerSignInManager;
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
        public async Task <IActionResult> SaveNewcutomer(Customer cus)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _customerManager.FindByEmailAsync(cus.Email);
                if (existingCustomer == null)
                {
                    await _customerManager.CreateAsync(cus);

                    ViewBag.NotificationMessage = "Customer registered successfully!";
                    ViewBag.NotificationType = "success";
                    return RedirectToAction("Cusolginsignup");
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
                return View("UserSignUp", cus);

            }

        }
        public IActionResult AdminSignUp()
        {
            return View();
        }
        public async Task<IActionResult >SaveAdminAndResturant(Restaurant res, Admin adm)
        {
            if (ModelState.IsValid)
            {
                var resturannam = await _context.Restaurant.FirstOrDefaultAsync(x => x.Name == res.Name);
                var Admininsystim = await _adminManager.FindByEmailAsync(adm.Email);
                if (resturannam == null && Admininsystim == null)
                {
                    
                    _context.Restaurant.Add(res);
                    _context.SaveChanges();
                    
                    var rres = _context.Restaurant.Where(x => x.Name == res.Name).FirstOrDefault();
                    
                    adm.RestaurantId = rres.Id;
                    adm.Restaurant = rres;

                    await _adminManager.CreateAsync(adm);
                    return RedirectToAction("ResturantLogIn", "Master");
                }
                else if (resturannam != null && Admininsystim != null)
                {
                    ViewBag.NotificationMessage = "The Resturant Name and Email already in the system";
                    ViewBag.NotificationType = "danger";
                }
                else if (resturannam != null)
                {
                    ViewBag.NotificationMessage = "The Resturant Name already in the system";
                    ViewBag.NotificationType = "danger";
                    //return View("UserSignUp", cus);
                }
                else
                {
                    ViewBag.NotificationMessage = "The Email already in the system";
                    ViewBag.NotificationType = "danger";
                }
            }
            else
            {
                ViewBag.NotificationMessage = "There are missing data.";
                ViewBag.NotificationType = "danger";
            }

            return View("AdminSignUp");
        }
        public IActionResult CustomerLogIn()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmCustomerLogIn(string email, string pass)
        {
            var existingCustomer = await _customerManager.FindByEmailAsync(email);
            if (existingCustomer != null)
            {
                var result = await _customerSignInManager.PasswordSignInAsync(existingCustomer, pass, false, false);
                if (!result.Succeeded)
                {
                    ViewBag.NotificationMessage = "Wrong Email or Password";
                    ViewBag.NotificationType = "danger";
                    return View("CustomerLogIn");
                }
                return RedirectToAction("index", "CustomerView", existingCustomer);
            }
            else
            {
                ViewBag.NotificationMessage = "Wrong Email or Password";
                ViewBag.NotificationType = "danger";
                return View("CustomerLogIn");
            }
        }
        public IActionResult ResturantLogIn()
        {
            return View();
        }
        public async Task<IActionResult> REsturantonerLogIn(string email, string pass)
        {
            var existingAdmin = await _adminManager.FindByEmailAsync(email);
            if (existingAdmin != null)
            {
                var result = await _adminSignInManager.PasswordSignInAsync(existingAdmin, pass, false, false);
                if (!result.Succeeded)
                {
                    ViewBag.NotificationMessage = "wrong email or password";
                    ViewBag.NotificationType = "danger";
                    return View("ResturantLogIn");
                }

                return RedirectToAction("AdminProfile", "Home", existingAdmin);
            }
            else
            {
                ViewBag.NotificationMessage = "wrong email or password";
                ViewBag.NotificationType = "danger";
                return View("ResturantLogIn");
            }

        }

    }
}



