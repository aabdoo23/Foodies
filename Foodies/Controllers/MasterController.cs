using Foodies.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class Master : Controller
    {
        Foodiesdbcontext context = new Foodiesdbcontext();
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
        public IActionResult SaveNewcutomer(Customer cus)
        {
            var existingCustomer = context.Customers.FirstOrDefault(x => x.CustomerEmail == cus.CustomerEmail);
            if (existingCustomer == null)
            {
                Customer customer = new Customer();
                customer.CustomerFirstName = cus.CustomerFirstName;
                customer.CustomerLastName = cus.CustomerLastName;
                customer.PhoneNumber = cus.PhoneNumber;
                customer.Password = cus.Password;
                customer.City = cus.City;
                customer.Street = cus.Street;
                customer.Building = cus.Building;
                customer.CustomerEmail = cus.CustomerEmail;
                context.Customers.Add(customer);
                context.SaveChanges();

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
        public IActionResult AdminSignUp()
        {
            return View();
        }
        public IActionResult SaveAdminAndResturant(Restaurant res, Admin adm)
        {
            var resturannam = context.Restaurants.FirstOrDefault(x => x.RestaurantName == res.RestaurantName);
            var Admininsystim = context.Admins.FirstOrDefault(x => x.Email == adm.Email);
            if (resturannam == null && Admininsystim == null)
            {
                Restaurant restaurant = new Restaurant();
                Admin newadmin = new Admin();
                restaurant.RestaurantName = res.RestaurantName;
                restaurant.Hotline = res.Hotline;
                restaurant.MinPrice = res.MinPrice;
                restaurant.MaxPrice = res.MaxPrice;
                restaurant.CusineType = res.CusineType;
                restaurant.Restorantphoto = res.Restorantphoto;
                context.Restaurants.Add(restaurant);
                context.SaveChanges();
                newadmin.AdminFirstName = adm.AdminFirstName;
                newadmin.AdminLastName = adm.AdminLastName;
                newadmin.PhoneNumber = adm.PhoneNumber;
                newadmin.Email = adm.Email;
                newadmin.Password = adm.Password;
                var rresid = context.Restaurants.Where(x => x.RestaurantName == res.RestaurantName).Select(x => x.RestaurantId).FirstOrDefault();
                newadmin.RestaurantId = rresid;
                context.Admins.Add(newadmin);
                context.SaveChanges();
                return View("Ressolginsignup");
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

            return View("AdminSignUp");
        }
        public IActionResult CustomerLogIn()
        {
            return View();
        }
        public IActionResult ConfirmCustomerLogIn(string email, string pass)
        {
            var existingCustomer = context.Customers.FirstOrDefault(x => x.CustomerEmail == email && x.Password == pass);
            if (existingCustomer != null)
            {
                return RedirectToAction("index", "CustomerView");
            }
            else
            {
                ViewBag.NotificationMessage = "wrong email or password";
                ViewBag.NotificationType = "danger";
                return View("CustomerLogIn");
            }


        }
        public IActionResult ResturantLogIn()
        {
            return View();
        }
        public IActionResult REsturantonerLogIn(string email, string pass)
        {
            var existingCustomer = context.Admins.FirstOrDefault(x => x.Email == email && x.Password == pass);
            if (existingCustomer != null)
            {
                return RedirectToAction("index", "CustomerView");
            }
            else
            {
                ViewBag.NotificationMessage = "wrong email or password";
                ViewBag.NotificationType = "danger";
                return View("CustomerLogIn");
            }


        }

    }
}



