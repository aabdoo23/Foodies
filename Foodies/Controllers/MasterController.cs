using Foodies.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class Master : Controller
    {
        FoodiesDbContext context = new FoodiesDbContext();
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
            var existingCustomer = context.Customer.FirstOrDefault(x => x.Email == cus.Email);
            if (existingCustomer == null)
            {
                Customer customer = new Customer();
                customer.FirstName = cus.FirstName;
                customer.LastName = cus.LastName;
                customer.PhoneNumber = cus.PhoneNumber;
                customer.Password = cus.Password;
                customer.City = cus.City;
                customer.Street = cus.Street;
                customer.Building = cus.Building;
                customer.Email = cus.Email;
                context.Customer.Add(customer);
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
            var resturannam = context.Restaurant.FirstOrDefault(x => x.Name == res.Name);
            var Admininsystim = context.Admin.FirstOrDefault(x => x.Email == adm.Email);
            if (resturannam == null && Admininsystim == null)
            {
                Restaurant restaurant = new Restaurant();
                Admin newadmin = new Admin();
                restaurant.Name = res.Name;
                restaurant.Hotline = res.Hotline;
                restaurant.MinPrice = res.MinPrice;
                restaurant.MaxPrice = res.MaxPrice;
                restaurant.CuisineType = res.CuisineType;
                restaurant.Photo = res.Photo;
                context.Restaurant.Add(restaurant);
                context.SaveChanges();
                newadmin.FirstName = adm.FirstName;
                newadmin.LastName = adm.LastName;
                newadmin.PhoneNumber = adm.PhoneNumber;
                newadmin.Email = adm.Email;
                newadmin.Password = adm.Password;
                var rresid = context.Restaurant.Where(x => x.Name == res.Name).Select(x => x.Id).FirstOrDefault();
                newadmin.RestaurantId = rresid;
                context.Admin.Add(newadmin);
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
            var existingCustomer = context.Customer.FirstOrDefault(x => x.Email == email && x.Password == pass);
            if (existingCustomer != null)
            {
              
                return RedirectToAction("index", "CustomerView",existingCustomer);
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
            var existingCustomer = context.Admin.FirstOrDefault(x => x.Email == email && x.Password == pass);
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



