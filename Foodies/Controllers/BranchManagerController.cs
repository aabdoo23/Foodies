using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodies.Controllers
{
    public class BranchManagerController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly ILogger<BranchManagerController> _logger;

        public BranchManagerController(FoodiesDbContext context, ILogger<BranchManagerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OrderList()
        {
            var customers = new List<Customer>
            {
                //new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" },
                //new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "0987654321" }
            };

            var orders = new List<Order> // Make sure this Order is from Foodies.Models
            {
                new Order
                {
                    Id = 1,
                    State = "Pending",
                    TotalPrice = 50.00m,
                    OrderDate = DateTime.Now.AddDays(-1),
                    Customer = customers[0] // Linking first customer
                },
                new Order
                {
                    Id = 2,
                    State = "Completed",
                    TotalPrice = 75.00m,
                    OrderDate = DateTime.Now.AddDays(-2),
                    Customer = customers[1] // Linking second customer
                }
            };

            return View(orders as IEnumerable<Order>);
        }
        //public async Task<IActionResult> OrderList()
        //{
        //    var branchManagerId = GetCurrentBranchManagerId(); // 

        //    var orders = await _context.Order
        //        .Include(o => o.Customer)
        //        .Include(o => o.Items)
        //        //.Where(o => o.BranchId == branchManagerId) esnure that the branch manager only see his branch's orders
        //        .ToListAsync();

        //    return View(orders);
        //}
        public IActionResult Details()
        {
            return View();
        }
        //public async Task<IActionResult> Details(int id)
        //{
        //    var order = await _context.Order
        //        .Include(o => o.Customer)
        //        .Include(o => o.Items)
        //        .FirstOrDefaultAsync(o => o.Id == id);

        //    if (order == null)
        //    {
        //        return NotFound(); 
        //    }

        //    return View(order);
        //}

        //public async Task<IActionResult> Profile(int id)
    //{
        //var branchManager = await _context.BranchManager
        //    .Include(b => b.Branch)
        //    .ThenInclude(r => r.Restaurant)
            //.SingleOrDefaultAsync(x => x.Id == id);

        //if (branchManager == null)
        //{
        //    return NotFound(); 
        //}

        //ViewBag.Restaurant = branchManager.Branch.Restaurant;
        //return View(branchManager); 
    //}

    // POST: /BranchManager/UpdateInfo
    [HttpPost]
    public IActionResult UpdateInfo(BranchManager manager)
    {
        var branchManager = _context.BranchManager.SingleOrDefault(x => x.Id == manager.Id);

        if (branchManager != null)
        {
            //branchManager.Username = manager.Username;
            //branchManager.Password = manager.Password;

            _context.SaveChanges();
            ViewBag.NotificationMessage = "Account details updated successfully!";
            ViewBag.NotificationType = "success";
        }
        else
        {
            ViewBag.NotificationMessage = "Branch manager not found!";
            ViewBag.NotificationType = "error";
        }

        return RedirectToAction("Profile", new { id = manager.Id });
    }

    // POST: /BranchManager/UpdateBranch
    [HttpPost]
    public IActionResult UpdateBranch(int id, Branch branch)
    {
        var existingBranch = _context.Branch.Include(b => b.Restaurant).SingleOrDefault(x => x.BranchId == id);

        if (existingBranch != null)
        {
            //existingBranch.BranchArea = branch.BranchArea;
            //existingBranch.BranchLocation = branch.BranchLocation;
            existingBranch.OpeningHour = branch.OpeningHour;
            existingBranch.ClosingHour = branch.ClosingHour;

            _context.SaveChanges();
            ViewBag.NotificationMessage = "Branch details updated successfully!";
            ViewBag.NotificationType = "success";
        }
        else
        {
            ViewBag.NotificationMessage = "Branch not found!";
            ViewBag.NotificationType = "error";
        }

        return RedirectToAction("Profile", new { id = existingBranch.BranchManager.Id });
       }
        private int GetCurrentBranchManagerId()
        {
            //log in??
            return 1;
        }



    }
}
