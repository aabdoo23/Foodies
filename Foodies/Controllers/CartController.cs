using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

public class CartController : Controller
    {
        private readonly FoodiesDbContext _context;
        static private int  cnt = 0;
        public CartController(FoodiesDbContext context)
        {
            _context = context;
        }

    //public IActionResult art(int id)
    //{

    //    return Content("NORO");
    //}
    public IActionResult addCart(int itemId)
    {
        CookieOptions options = new CookieOptions();
        options.Expires = DateTimeOffset.Now.AddDays(5);
        //replace key with customer id?
        Response.Cookies.Append((++cnt).ToString(), itemId.ToString(),options);
        if (itemId == 0)
        {
            return Content("No itemId received");
        }

        // Output the cookie list along with the itemId for testing purposes
        return Content("The is");
    }

    public IActionResult removeCart(int itemId, bool ?dec)
    {
        foreach (var cookie in Request.Cookies)
        {
            // Check if the cookie value matches the value to remove
            if (cookie.Value == itemId.ToString())
            {
                // Remove the cookie by its key
                Response.Cookies.Delete(cookie.Key);
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
    public IActionResult cart()
    {
        List<MenuItem> myCart = new List<MenuItem>();
        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Value, out int itemId))
            {
                MenuItem menuItem = _context.MenuItem
                    .Where(x => x.Id == itemId)
                    .SingleOrDefault();
                myCart.Add(menuItem);
            }
            else {
                Response.Cookies.Delete(cookie.Key);
            }
        }
        return View(myCart);
    }


}




