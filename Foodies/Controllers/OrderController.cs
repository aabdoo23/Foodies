using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

public class OrderController : Controller
{
    private readonly FoodiesDbContext _context;
    static private int cnt = 0;
    private List<MenuItem> myCart = new List<MenuItem>();

    public OrderController(FoodiesDbContext context)
    {
        _context = context;
    }


    //checkout
    [HttpPost]
    public IActionResult checkout(int total)
    {
        ViewBag.Total = total.ToString();
        //return Content($"noura{ViewBag.Total}");
        return View();
    }

    public IActionResult order(int total)
    {
        //id , State , total , date, paymentid , customerid, branch id
        Order order = new Order();
        order.State = "Pending";
        order.TotalPrice = total;
        order.OrderDate = DateTime.Now;
        //order.Payment 

        //Customer
        var id = HttpContext.Session.GetInt32("Customer");
        order.Customer = _context.Customer.Where(x => x.Id == id).SingleOrDefault();
        //order.Branch


        order.Items = myCart;

        //last add order in menuitem model
        //empty cart
        foreach (var item in myCart)
        {
            item.Orders.Add(order);
            removeCart(item.Id,false);
        }

        return Content("still not finished action");
    }


    //Cart
    public IActionResult addCart(int itemId)
    {
        CookieOptions options = new CookieOptions();
        options.Expires = DateTimeOffset.Now.AddDays(5);
        //replace key with customer id?
        Response.Cookies.Append((++cnt).ToString(), itemId.ToString(), options);
        if (itemId == 0)
        {
            return Content("No itemId received");
        }

        // Output the cookie list along with the itemId for testing purposes
        return Content("The is");
    }


    public IActionResult removeCart(int itemId, bool? dec)
    {
        foreach (var cookie in Request.Cookies)
        {
            // Check if the cookie value matches the value to remove
            if (cookie.Value == itemId.ToString())
            {
                // Remove the cookie by its key
                Response.Cookies.Delete(cookie.Key);
                MenuItem menuItem = _context.MenuItem
                    .Where(x => x.Id == itemId)
                    .SingleOrDefault();
                myCart.Remove(menuItem);
                if (dec == true)
                {
                    break;
                }

            }
        }

        // Output the cookie list along with the itemId for testing purposes
        return Content("The is decrease");
    }

    public IActionResult History()
    {
        return View();
    }

    //view cart
    public IActionResult cart()
    {
        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Value, out int itemId))
            {
                MenuItem menuItem = _context.MenuItem
                    .Where(x => x.Id == itemId)
                    .SingleOrDefault();
                myCart.Add(menuItem);
            }
            else
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }
        return View(myCart);
    }


}




