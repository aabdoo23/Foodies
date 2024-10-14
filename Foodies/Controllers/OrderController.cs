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
using Azure;
using GoogleApi.Entities.Maps.Common;
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


    public (double latitude, double longitude)? ExtractCoordinates(string url)
    {
        string pattern = @"@(-?\d+\.\d+),(-?\d+\.\d+)";
        Response.Cookies.Append("url", url);
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

    //resolve make it longer link
    //use logt and lat function
    //put it in final function
    public async Task<JObject> GetDistanceTime(double l1, double g1, double l2, double g2)
    {
        String apiKey = "5c2a1476c0e97f202c537b7e0459338cb9792efca2e0b763809c278a810abe74";
        Hashtable ht = new Hashtable();
        ht.Add("engine", "google_maps_directions");
        ht.Add("start_coords", $"{l1}, {g1}");
        ht.Add("end_coords", $"{l2}, {g2}");

        GoogleSearch search = new GoogleSearch(ht, apiKey);
        JObject data = search.GetJson();
        var directions = data["directions"];
        if (directions != null && directions.HasValues)
        {

            var firstDirection = directions[0];

                // Extract travel data


                // Extract travel data
                var distance = firstDirection["formatted_distance"]?.ToString();
                var time = firstDirection["formatted_duration"]?.ToString();


                // Return JSON object with distance and time as strings
                return JObject.FromObject(new { distance = distance, time = time });
            
            }
        return null;
    }


    public async Task<int> proceedDistanceTime(int resId)
    {
        var userId = _userManager.GetUserId(User);
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a => a.Address).FirstOrDefaultAsync();
        
        Restaurant rest = await _context.Restaurant.Where(x => x.Id == resId).Include(b => b.Branches).ThenInclude(b => b.Address).FirstOrDefaultAsync();

        
        //double Bdist =double.MaxValue;
        //int Btime = int.MaxValue;
        
        //int branchID = rest.Branches[0].BranchId;

        Dictionary<int, (string dist, string time)> data = new Dictionary<int, (string, string)>();

        var coordinateCustomer = ExtractCoordinates(customer.Address.Location);

        foreach (var b in rest.Branches)
        {
            //string destination = await _mapService.ResolveGoogleMapsLink(b.Address.Location);
            var coordinateBranch = ExtractCoordinates(b.Address.Location);

            //string location = await _mapService.ResolveGoogleMapsLink(customer.Address.Location);
            //if (Request.Cookies["distance"]!=null && double.Parse(Request.Cookies["distance"]) < dist )
            //{
            //    dist = double.Parse(Request.Cookies["distance"]);
            //    branchID = b.BranchId;
            //}
             var result = await GetDistanceTime(coordinateCustomer.Value.latitude, coordinateCustomer.Value.longitude,coordinateBranch.Value.latitude, coordinateBranch.Value.longitude );
            //Response.Cookies.Append("br", result["distance"].ToString());

            if (result["distance"] != null && result["time"] != null)
            {

                data[b.BranchId] = (result["distance"].ToString(), result["time"].ToString());    // Population, Area in km²

            }

        }

        //logic for sorting
        List<int> timeList = new List<int>();
        Dictionary<int, (string dist, int time)> data2 = new Dictionary<int, (string, int)>();
        bool entered = false;
        foreach (var t in data)
        {
            var sp = t.Value.time.Split(' ');
            if (sp[1] == "min" || sp[1] == "mins")
            {
                data2[t.Key] = (t.Value.dist,int.Parse(sp[0]));
                entered = true;
            }
            if (sp[1] == "hr" || sp[1] == "hrs")
            {
                data2[t.Key] = (t.Value.dist, int.Parse(sp[0])*60 );
                entered = true;
            }
        }
        var sorted = data2.OrderBy(d => d.Value.time);
        var distanceList = sorted.Select(d => $"{d.Key}:{d.Value.time}:{d.Value.dist}").ToList();
        string distanceString = string.Join(",", distanceList);

        Response.Cookies.Append("uu", distanceString);
        Response.Cookies.Append("time", sorted.First().Value.time.ToString());

        Response.Cookies.Append("distance", sorted.First().Value.dist.ToString());


        return sorted.First().Key;
    }


    [HttpPost]
    public async Task<IActionResult> checkout(int total)
    {
        ViewBag.Total = total.ToString();
        int restID = int.Parse(Request.Cookies["restId"]);
        //Response.Cookies.Append("distance", $"{double.MaxValue}");
        //Response.Cookies.Append("time", $"{int.MaxValue}");

        var userId = _userManager.GetUserId(User);
        Customer customer = await _context.Customer.Where(x => x.Id == userId).Include(a => a.Address).FirstOrDefaultAsync();
        ViewBag.fav = customer;

        int branchID =await  proceedDistanceTime(restID);
        Branch branch = await _context.Branch
            .Where(x => x.BranchId == branchID)
            .Include(a => a.Address).FirstOrDefaultAsync();

        Response.Cookies.Append("bID", branch.BranchId.ToString());

        ViewBag.distance = Request.Cookies["distance"];
        ViewBag.time = Request.Cookies["time"];

        return View(branch);
    }



    [HttpPost]
    public async Task<IActionResult> AddCard(Card card)
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
        int branchID = int.Parse(Request.Cookies["bID"]);
        Branch branch = await _context.Branch.Where(x=> x.BranchId == 1).SingleOrDefaultAsync();
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




