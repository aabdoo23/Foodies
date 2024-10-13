using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CreditCardValidator;
using Foodies.Models;
using Foodies.Migrations;
using System.Collections.Generic;
using Foodies.Controllers;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.CodeAnalysis.Operations;
public class OrderController : Controller
{
    private readonly FoodiesDbContext _context;
    //static private int cnt = 0;
    private List<MenuItem> myCart = new List<MenuItem>();
    private readonly UserManager<IdentityUser> _userManager;
    private readonly GeminiController _geminiController;


    public OrderController(FoodiesDbContext context ,UserManager<IdentityUser> userManager,
        GeminiController geminiController)
    {
        _context = context;
        _userManager = userManager;
        _geminiController = geminiController;
    }

    public async Task<int> getNearestBranch(int resId)
    {
        List<double> closest= new List<double>();
        int branchId=1;
        double val = double.MaxValue;
        var userId = _userManager.GetUserId(User);
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a=>a.Address).FirstOrDefaultAsync();
        Restaurant rest = _context.Restaurant.Where(x => x.Id == resId).Include(b => b.Branches).FirstOrDefault();
        foreach (var b in rest.Branches)
        {
            string result = await _geminiController.Generate($"distance%20between%20{b.Address.Location}%20and%20{customer.Address.Location}%20in%20km%20short%20answer");
            string numericPart = Regex.Match(result, @"\d+(\.\d+)?").Value;
            double dist = double.Parse(numericPart);
            if(val > dist)
            {
                val = dist;
                branchId = b.BranchId;
            }
        }
        Response.Cookies.Append("distance", val.ToString());
        return branchId;
    }
    public async Task<int> getTimeNearestBranch(int branchId)
    {
        
        var userId = _userManager.GetUserId(User);
        Branch b = _context.Branch.Where(x => x.BranchId == branchId).SingleOrDefault();
        int time = 0;
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a => a.Address).FirstOrDefaultAsync();
        
        string result = await _geminiController.Generate($"time%20driving%20between%20{b.Address.Location}%20and%20{customer.Address.Location}%20in%20min%20short%20answer");
        string numericPart = Regex.Match(result, @"\d+(\.\d+)?").Value;
        time = int.Parse(numericPart);
           
        
        return time;
    }

    [HttpPost]
    public async Task< IActionResult> AddCard(Card card)
    {
        // Check if the card number is null or empty
        if (card.CardNumber == null)
        {
            return BadRequest(new { success = false, message = "Card number is required." });
        }
        var userId = _userManager.GetUserId(User);
        Customer customer = await _context.Customer.Where(x => x.Id == userId).FirstOrDefaultAsync();
        var creditCardDetector = new CreditCardDetector(card.CardNumber);

        if (creditCardDetector.IsValid())
        {
            // Convert the card type to string
            string cardType = creditCardDetector.Brand.ToString(); // e.g., "Visa", "MasterCard"
            card.Type = cardType;
            card.customer = customer;
            customer.card = card;
            _context.Card.Add(card);
            
            Response.Cookies.Append("CardId", card.Id.ToString());
            _context.SaveChanges();
            return RedirectToAction("UserView", "Home");
            //return Content($"{card.CardNumber} fofo");
        }
        ViewBag.NotificationMessage = "Card is not Valid";
        ViewBag.NotificationType = "danger";

        return RedirectToAction("UserView", "Home");

    }


    
    [HttpPost]
    public IActionResult OrderView(int total, string paymentMethod)
    {
        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem menuItem = _context.MenuItem
                    .Where(x => x.Id == int.Parse(cookie.Value))
                    .SingleOrDefault();
                myCart.Add(menuItem);
            }
        }
        Response.Cookies.Append("paymentmethod", paymentMethod);
        //return Content($"{paymentMethod}fef{total}");
        return View(myCart);
    }

    //checkout
    [HttpPost]
    public async Task<IActionResult> order(int total, string paymentMethod)
    {
        ////id , State , total , date, paymentid , customerid, branch id
        Order order = new Order();
        order.State = "Pending";
        order.TotalPrice = total;
        order.OrderDate = DateTime.Now;
        ////order.Payment 

        ////Customer
        var userId = _userManager.GetUserId(User);
        Customer Cust = _context.Customer.Where(x => x.Id == userId)
            .Include(o=> o.Orders)
            .SingleOrDefault();

        order.Customer = Cust;



        

        //payment
        Payment payment = new Payment
        {
            PaymentDate = DateTime.Now,
            PaymentMethod = Request.Cookies["paymentMethod"],
            Amount = total,
            Order = order
        };
        //not tested yet
        if (payment.PaymentMethod == "Card")
        {
            int cardId = int.Parse(Request.Cookies["CardId"]);

            Card card = _context.Card.Where(x => x.CustomerId == userId).Include(p => p.payments).SingleOrDefault();
            payment.card = card;
            card.payments.Add(payment);
        }
        order.Payment = payment;

        ////order.Branch
        //order.Branch = 
        int restID = int.Parse(Request.Cookies["restId"]);
        //Restaurant rest = _context.Restaurant.Where(x => x.Id == int.Parse(Request.Cookies["restId"]))
        //    .Include(b => b.Branches).SingleOrDefault();
        //return Content($"{rest.Branches[0]} hoho");
        int branchID = await getNearestBranch(restID);
        Branch branch = _context.Branch.Where(x=> x.BranchId == branchID).SingleOrDefault();
        order.Branch = branch;

        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem item = _context.MenuItem.Where(x => x.Id == int.Parse(cookie.Value)).Include(o => o.Orders).SingleOrDefault();

                myCart.Add(item);
                Response.Cookies.Delete(cookie.Key);
            }


        }
        // last add order in menuitem model
        //empty cart
        order.Items = new List<MenuItem>(myCart);
        
        foreach (var item in myCart)
        {
            //return Content($"{item.Id} hoho");

            item.Orders.Add(order);

        }

        
        Cust.Orders.Add(order);
        branch.Orders.Add(order);
        _context.Order.Add(order);
        _context.SaveChanges();
        
        myCart.Clear();

        return Content($"{order.Id} hoho");


    }

    [HttpPost]
    public IActionResult checkout(int total)
    {
        ViewBag.Total = total.ToString();
        //return Content($"noura{ViewBag.Total}");
        return View();
    }
    

  
    //Cart
    public IActionResult addCart(int itemId)
    {
        CookieOptions options = new CookieOptions();
        options.Expires = DateTimeOffset.Now.AddDays(5);
        int cnt = 1;
        bool entered = false;
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
            if (cookie.Value == itemId.ToString() && int.TryParse(cookie.Key, out int id))
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
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem menuItem = _context.MenuItem
                    .Where(x => x.Id == int.Parse(cookie.Value)).Include(r=>r.Resturant)
                    .SingleOrDefault();
                myCart.Add(menuItem);
                if(menuItem == null)
                {
                    return Content($"{cookie.Value}");
                }
                Response.Cookies.Append("restId", $"{menuItem.ResturantId}");
            }
            //else
            //{
            //    Response.Cookies.Delete(cookie.Key);
            //}
        }
        
            return View(myCart);


        
    }


}




