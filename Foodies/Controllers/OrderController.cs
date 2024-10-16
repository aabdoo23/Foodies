using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CreditCardValidator;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using SerpApi;
using System.Collections;
using FirebaseAdmin.Messaging;
using Azure;
using GoogleApi.Entities.Maps.Common;
using Foodies.Interfaces.Repositories;
using Foodies.Repositories;
using System.Collections.Generic;
public class OrderController : Controller
{
    //private readonly FoodiesDbContext _context;
    //static private int cnt = 0;
    private List<MenuItem> myCart = new List<MenuItem>();
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICardRepository _cardRepository;

    private readonly MapService _mapService;


    public OrderController(
        UserManager<IdentityUser> userManager,
        IRestaurantRepository restaurantRepository,
        IMenuItemRepository menuItemRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IOrderRepository orderRepository,
        ICardRepository cardRepository,

        MapService mapService)
    {
        _restaurantRepository = restaurantRepository;
        _menuItemRepository = menuItemRepository;
        _userManager = userManager;
        _mapService = mapService;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _cardRepository = cardRepository;
        _orderRepository = orderRepository;
    }

    public async Task<IActionResult> history()
    {
        var userid = _userManager.GetUserId(User);
        var orders = await _orderRepository.GetAllcustomeridwithMenu(userid);
        return View(orders);
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

            var distance = firstDirection["formatted_distance"]?.ToString();
            var time = firstDirection["formatted_duration"]?.ToString();


            // Return JSON object with distance and time as strings
            return JObject.FromObject(new { distance = distance, time = time });
           
         }
        return null;
    }


    public async Task<string> proceedDistanceTime(string resId)
    {
        var userId = _userManager.GetUserId(User);
        Customer customer = await _customerRepository.GetById(userId);
        
        Restaurant rest = await _restaurantRepository.GetByIdWithBranchesIncludeAddress(resId);

        Dictionary<string, (string dist, string time)> data = new Dictionary<string, (string, string)>();

        var coordinateCustomer = ExtractCoordinates(customer.Address.Location);

        foreach (var b in rest.Branches)
        {
            //resolved url too many requests 409
            //string destination = await _mapService.ResolveGoogleMapsLink(b.Address.Location);
            //string location = await _mapService.ResolveGoogleMapsLink(customer.Address.Location);

            var coordinateBranch = ExtractCoordinates(b.Address.Location);

            var result = await GetDistanceTime(coordinateCustomer.Value.latitude, coordinateCustomer.Value.longitude,coordinateBranch.Value.latitude, coordinateBranch.Value.longitude );
            Response.Cookies.Append($"br{b.Id}", $"{coordinateBranch.Value.latitude}{coordinateBranch.Value.longitude}");

            if (result["distance"] != null && result["time"] != null)
            {

                data[b.Id] = (result["distance"].ToString(), result["time"].ToString());    // Population, Area in km²

            }

        }

        //logic for sorting
        Dictionary<string, (string dist, int time)> data2 = new Dictionary<string, (string, int)>();
        foreach (var t in data)
        {
            var sp = t.Value.time.Split(' ');
            if (sp[1] == "min" || sp[1] == "mins")
            {
                data2[t.Key] = (t.Value.dist,int.Parse(sp[0]));
            }
            if (sp[1] == "hr" || sp[1] == "hrs")
            {
                data2[t.Key] = (t.Value.dist, int.Parse(sp[0])*60 );
            }
        }
        var sorted = data2.OrderBy(d => d.Value.time);
        var distanceList = sorted.Select(d => $"{d.Key}:{d.Value.time}:{d.Value.dist}").ToList();
        string distanceString = string.Join(",", distanceList);

        //for debugging purposes -dont delete it
        Response.Cookies.Append("uu", distanceString);


        Response.Cookies.Append("time", data[sorted.First().Key].time);
        Response.Cookies.Append("distance", sorted.First().Value.dist);


        return $"{sorted.First().Key}:{data[sorted.First().Key].time}:{sorted.First().Value.dist}";
    }


    [HttpPost]
    public async Task<IActionResult> checkout(int total)
    {

        ViewBag.Total = total;
        string restID = Request.Cookies["restId"];

        var userId = _userManager.GetUserId(User);
        Customer customer = await _customerRepository.GetById(userId);
        ViewBag.fav = customer;

        
        string data = await proceedDistanceTime(restID);
        var arrayData = data.Split(':');
        string branchID = arrayData[0];
        Branch branch = await _branchRepository.GetById(branchID);

        Response.Cookies.Append("bID", branch.Id);
        ViewBag.distance = arrayData[2];
        ViewBag.time = arrayData[1];
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
        Customer customer = await _customerRepository.GetById(userId);
        var creditCardDetector = new CreditCardDetector(card.CardNumber);

        if (creditCardDetector.IsValid())
        {
            // Convert the card type to string
            string cardType = creditCardDetector.Brand.ToString(); // e.g., "Visa", "MasterCard"
            card.Type = cardType;
            card.customer = customer;
            customer.card = card;
            await _cardRepository.Create(card);

            Response.Cookies.Append("CardId", card.Id);
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
                MenuItem menuItem = await _menuItemRepository.GetById(cookie.Value);
                myCart.Add(menuItem);
            }
        }
        ViewBag.total = total;
        Response.Cookies.Append("paymentmethod", paymentMethod);
        return View(myCart);
    }

    //checkout
    [HttpPost]
    public async Task<IActionResult> order(int total)
    {
        ////id , State , total , date, paymentid , customerid, branch id
        Order order = new Order();
        order.State = "Pending";
        order.TotalPrice = total;
        order.OrderDate = DateTime.Now;
        ////order.Payment 

        ////Customer
        var userId = _userManager.GetUserId(User);
        Customer Cust = await _customerRepository.GetByIdIncludeOrders(userId);
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
            string cardId = Request.Cookies["CardId"];

            Card card = await _cardRepository.GetCardByCustomerId(userId);
            payment.card = card;
            card.payments.Add(payment);
        }
        order.Payment = payment;

        //string restID = Request.Cookies["restId"];

        string branchID = Request.Cookies["bID"];
        Branch branch = await _branchRepository.GetByIdIcludeOrders(branchID);
        order.Branch = branch;

        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem item = await _menuItemRepository.GetByIdWithOrders(cookie.Value);

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
        payment.Order = order;
        Cust.Points+= 100;
    
        await _orderRepository.Create(order);
        myCart.Clear();

        return RedirectToAction("restaurant", "menu");


    }

    

  
    //Cart
    public async Task<IActionResult> addCart(string itemId)
    {
        CookieOptions options = new CookieOptions();
        options.Expires = DateTimeOffset.Now.AddDays(5);
        int cnt = 1;
        //bool entered = false;
        //replace key with customer id?
        
        Response.Cookies.Append((++cnt).ToString(), itemId.ToString(), options);
                 
        if (itemId == null)
        {
            return Content("No itemId received");
        }


        // Output the cookie list along with the itemId for testing purposes
        return Content("The is");
    }


    public async Task<IActionResult> removeCart(string itemId, bool? dec)
    {
        foreach (var cookie in Request.Cookies)
        {
            // Check if the cookie value matches the value to remove
            if (cookie.Value == itemId.ToString() && int.TryParse(cookie.Key, out int id))
            {
                // Remove the cookie by its key
                Response.Cookies.Delete(cookie.Key);
                MenuItem menuItem = await _menuItemRepository.GetById(itemId.ToString());
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

    //view cart
    public async Task<IActionResult> cart()
    {
        foreach (var cookie in Request.Cookies)
        {
            if (int.TryParse(cookie.Key, out int id))
            {
                MenuItem menuItem = await _menuItemRepository.GetById(cookie.Value);
                myCart.Add(menuItem);
                if(menuItem == null)
                {
                    return Content($"{cookie.Value}");
                }
                Response.Cookies.Append("restId", $"{menuItem.ResturantId}");
            }
        }
        
            return View(myCart);

    }


}




