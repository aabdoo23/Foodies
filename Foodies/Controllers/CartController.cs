using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class CartController : Controller
    {
        private readonly FoodiesDbContext _context;
        static private int  cnt = 0;
        public CartController(FoodiesDbContext context)
        {
            _context = context;
        }


    public IActionResult art(int itemId) {

        Response.Cookies.Append((++cnt).ToString(), itemId.ToString());
        if (itemId == 0)
        {
            return Content("No itemId received");
        }

        // Output the cookie list along with the itemId for testing purposes
        return Content($"The bobo is {Request.Cookies["3"]}");
    }
    public IActionResult History()
    {
        return View();
    }
    public IActionResult Index()
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
            Response.Cookies.Delete(cookie.Key);
        }
        return View(myCart);
    }
    public IActionResult ff()
    {
        // Retrieve the list of item IDs from the cookie
        var cartItemIdsCookie = Request.Cookies["cartItemIds"];

        if (!string.IsNullOrEmpty(cartItemIdsCookie))
        {
            // Split the comma-separated IDs into an array
            var itemIds = cartItemIdsCookie.Split(',').Select(int.Parse).ToList();

            // Retrieve the MenuItem objects corresponding to the IDs
            var cartItems = _context.MenuItem.Where(item => itemIds.Contains(item.Id)).ToList();

            if (cartItems.Any())
            {
                // If items are found, pass them to the view
                return View(cartItems);
            }
        }

        // If no items are found or cookie is empty, return the EmptyCart view
        return View("EmptyCart");
    }

}




