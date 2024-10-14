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
using GeminiTextGenerator.Controllers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using SerpApi;
using System.Collections;
using FirebaseAdmin.Messaging;
public class OrderController : Controller
{
    private readonly FoodiesDbContext _context;
    //static private int cnt = 0;
    private List<MenuItem> myCart = new List<MenuItem>();
    private readonly UserManager<IdentityUser> _userManager;
    //private readonly GeminiService _geminiService;
    private readonly MapService _mapService;


    public OrderController(FoodiesDbContext context ,UserManager<IdentityUser> userManager,
        MapService mapService)
    {
        _context = context;
        _userManager = userManager;
        _mapService = mapService;
    }
    public async Task<string> Generate(string prompt)
    {
        //var result = await _geminiService.GenerateContentAsync(prompt);
        return "s";
    }
    public async Task<int> getNearestBranch(int resId)
    {
        List<double> closest= new List<double>();
        int branchId=1;
        double val = double.MaxValue;
        var userId = _userManager.GetUserId(User);
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a=>a.Address).FirstOrDefaultAsync();
        Restaurant rest = await _context.Restaurant.Where(x => x.Id == resId).Include(b => b.Branches).ThenInclude(b => b.Address).FirstOrDefaultAsync();
        foreach (var b in rest.Branches)
        {
            string result = await Generate($"distance%20between%20{b.Address.Location}%20and%20{customer.Address.Location}%20in%20km%20short%20answer");
            double dist = double.Parse(result.Split(' ')[0]);
            if (val > dist)
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
        Branch b = await _context.Branch.Where(x => x.BranchId == branchId).FirstOrDefaultAsync();
        int time = 0;
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a => a.Address).FirstOrDefaultAsync();
        
        string result = await Generate($"time%20driving%20between%20{b.Address.Location}%20and%20{customer.Address.Location}%20in%20min%20short%20answer");
        string numberString = new string(result.TakeWhile(char.IsDigit).ToArray());
        time = int.Parse(numberString);
        
        
        return  time;
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
            await _context.Card.AddAsync(card);
            
            Response.Cookies.Append("CardId", card.Id.ToString());
            await _context.SaveChangesAsync();
            return RedirectToAction("UserView", "Home");
            //return Content($"{card.CardNumber} fofo");
        }
        ViewBag.NotificationMessage = "Card is not Valid";
        ViewBag.NotificationType = "danger";

        return RedirectToAction("UserView", "Home");

    }
   

    public (double latitude, double longitude)? ExtractCoordinates(string url)
    {
        string pattern = @"@(-?\d+\.\d+),(-?\d+\.\d+)";
        Match match = Regex.Match(url, pattern);

        if (match.Success)
        {
            double latitude = double.Parse(match.Groups[1].Value);
            double longitude = double.Parse(match.Groups[2].Value);
            return (latitude, longitude);
        }

        return null;
    }
    //

    //[HttpGet]
    //resolve make it longer link
    //use logt and lat function
    //put it in final function
    public async Task<IActionResult> test()
    {
        // Resolve the first location (Karam el Sham)
        //string location = await _mapService.ResolveGoogleMapsLink("https://maps.app.goo.gl/LdcBPFoKVhUztVd96");

        //// Extract coordinates from the first location
        //var coordinates = ExtractCoordinates(location);

        //// Resolve the second location (Campus)
        //string destination = await _mapService.ResolveGoogleMapsLink("https://maps.app.goo.gl/NrDkdkFg1FwEejmU6");

        //// Extract coordinates from the second location
        //var coordinates2 = ExtractCoordinates(destination);

        //// Check if both sets of coordinates are valid
        //if (coordinates.HasValue && coordinates2.HasValue)
        //{

        // Get the total distance between the two coordinates
        //string totalDistance = await _mapService.GetTotalDistanceAsync(
        //    30.3017, -97.7408,
        //    40.3017, -87.7408
        //);

        String apiKey = "5c2a1476c0e97f202c537b7e0459338cb9792efca2e0b763809c278a810abe74";
        Hashtable ht = new Hashtable();
        ht.Add("engine", "google_maps_directions");
        ht.Add("start_coords", "30.01711724128, 31.18670409319749");
        ht.Add("end_coords", "30.058726995198423, 31.240605762125483");

            GoogleSearch search = new GoogleSearch(ht, apiKey);
            JObject data = search.GetJson();
            var directions = data["directions"];
        if (directions != null && directions.HasValues)
        {
            // Access the first direction (if multiple exist)
            var firstDirection = directions[0];

            // Now, check if the "trips" array exists in the first direction
            var trips = firstDirection["trips"] as JArray;

            if (trips != null && trips.HasValues)
            {
                var firstTrip = trips[0];

                // Extract travel data
                var travelMode = firstTrip["travel_mode"]?.ToString();
                var title = firstTrip["title"]?.ToString();
                var distance = firstTrip["distance"]?.ToString();
                var duration = firstTrip["duration"]?.ToString();

                // Return the extracted information as part of the response
                return Content($"Travel Mode: {travelMode}, Title: {title}, Distance: {distance}, Duration: {duration}");
            }
            else
            {
                return Content("No trips found in the first direction.");
            }
        }
        else
        {
            return Content("No directions found.");
        }
        //}
        //return Content("no if");



    
        //catch (SerpApiSearchException ex)
        //{
        //    Console.WriteLine("Exception:");
        //    Console.WriteLine(ex.ToString());

        //}
        //}

        //// Return error message if either set of coordinates is invalid
        //return Content("Invalid URL or coordinates not found.");
    }


    [HttpPost]
    public async Task<IActionResult> checkout(int total)
    {
        ViewBag.Total = total.ToString();
        int restID = int.Parse(Request.Cookies["restId"]);
        int branchID = await getNearestBranch(restID);

        var userId = _userManager.GetUserId(User);
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a => a.Address).FirstOrDefaultAsync();

        ViewBag.fav = customer;
        Branch branch = await _context.Branch
            .Where(x => x.BranchId == branchID)
            .Include(a => a.Address).FirstOrDefaultAsync();

        ViewBag.distance = Request.Cookies["distance"];
        ViewBag.time = await getTimeNearestBranch(branchID);

        return View(branch);
    }

    
    
    [HttpPost]
    public async Task<IActionResult> OrderView(int total, string paymentMethod)
    {
        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem menuItem = await _context.MenuItem
                    .Where(x => x.Id == int.Parse(cookie.Value))
                    .FirstOrDefaultAsync();
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
        Customer Cust = await _context.Customer.Where(x => x.Id == userId)
            .Include(o=> o.Orders)
            .SingleOrDefaultAsync();

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

            Card card = await _context.Card.Where(x => x.CustomerId == userId).Include(p => p.payments).SingleOrDefaultAsync();
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
        Branch branch = await _context.Branch.Where(x=> x.BranchId == branchID).SingleOrDefaultAsync();
        order.Branch = branch;

        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem item = await _context.MenuItem.Where(x => x.Id == int.Parse(cookie.Value)).Include(o => o.Orders).SingleOrDefaultAsync();


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
        await _context.Order.AddAsync(order);
        await _context.SaveChangesAsync();
        
        myCart.Clear();

        return Content($"{order.Id} hoho");


    }

    

  
    //Cart
    public async Task<IActionResult> addCart(int itemId)
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


    public async Task<IActionResult> removeCart(int itemId, bool? dec)
    {
        foreach (var cookie in Request.Cookies)
        {
            // Check if the cookie value matches the value to remove
            if (cookie.Value == itemId.ToString() && int.TryParse(cookie.Key, out int id))
            {
                // Remove the cookie by its key
                Response.Cookies.Delete(cookie.Key);
                MenuItem menuItem = await _context.MenuItem
                    .Where(x => x.Id == itemId)
                    .SingleOrDefaultAsync();
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
    public async Task<IActionResult> cart()
    {
        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem menuItem = await _context.MenuItem
                    .Where(x => x.Id == int.Parse(cookie.Value)).Include(r=>r.Resturant)
                    .SingleOrDefaultAsync();
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




