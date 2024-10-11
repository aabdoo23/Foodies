using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CreditCardValidator;
using Foodies.Models;
public class OrderController : Controller
{
    private readonly FoodiesDbContext _context;
    static private int cnt = 0;
    private List<MenuItem> myCart = new List<MenuItem>();
    private readonly UserManager<IdentityUser> _userManager;


    public OrderController(FoodiesDbContext context ,UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public IActionResult CardPayment(Card card)
    {
        // Check if the card number is null or empty
        if (string.IsNullOrWhiteSpace(card.CardNumber))
        {
            return BadRequest(new { success = false, message = "Card number is required." });
        }

        var creditCardDetector = new CreditCardDetector(card.CardNumber);

        if (creditCardDetector.IsValid())
        {
            // Convert the card type to string
            string cardType = creditCardDetector.Brand.ToString(); // e.g., "Visa", "MasterCard"
            card.Type = cardType;
            _context.Card.Add(card);
            Response.Cookies.Append("CardId", card.Id.ToString());

            return Ok(new { success = true, message = $"Card is valid: {cardType}" });
        }
        else
        {
            return BadRequest(new { success = false, message = "Invalid credit card number." });
        }
    }

    
    public IActionResult Payment(Payment payment) { 
        if(payment.PaymentMethod == "Card")
        {
            int cardId = int.Parse(Request.Cookies["CardId"]);

            Card card = _context.Card.Where(x=>x.Id == cardId).Include(p=>p.payments).SingleOrDefault();
            payment.card = card;
            card.payments.Add(payment);
        }
        _context.Payment.Add(payment);
        Response.Cookies.Append("PaymentId", payment.Id.ToString());

        return Content("payment done done done");
    }

    //checkout
    
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
        Customer Cust = _context.Customer.Where(x => x.Id == userId)
            .Include(o=> o.Orders)
            .SingleOrDefault();

        order.Customer = Cust;



        ////order.Branch
        //order.Branch = 
        Branch branch = myCart[0].Resturant.Branches[0];
        order.Branch = branch;

        //payment
        int paymentId = int.Parse(Request.Cookies["PaymentId"]);
        Payment payment = _context.Payment.Where(x => x.Id == paymentId).SingleOrDefault();
        order.PaymentId = paymentId;
        order.Payment = payment;
        

        //last add order in menuitem model
        //empty cart
        order.Items = new List<MenuItem>(myCart);

        foreach (var item in myCart)
        {
            item.Orders.Add(order);
            removeCart(item.Id,false);
        }


        payment.Order = order;
        Cust.Orders.Add(order);
        branch.Orders.Add(order);

        return Content("still not finished action");
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
    public IActionResult cart(int ?total)
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
            //else
            //{
            //    Response.Cookies.Delete(cookie.Key);
            //}
        }
        if(total == null)
        {
            return View(myCart);

        }
        else
        {
            return View("OrderView", myCart);

        }
    }


}




