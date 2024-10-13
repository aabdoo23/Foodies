using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public IActionResult OrderList(int id)
        {
           
            var oredr = _context.Order.Where(x => x.Branch.BranchId == id).Include(x => x.Items).ToList();
            
            return View(oredr as IEnumerable<Order>);
        }
        #region no
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
        #endregion
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Order.Include(o => o.Customer).Include(c=>c.Items).FirstOrDefaultAsync(o => o.Id == id);
       
            return View(order);
        }
        public async Task<IActionResult> Changestate(int id, string state)
        {
            var order = _context.Order.Include(x=>x.Branch).SingleOrDefault(x => x.Id == id);
            int idd=order.Branch.BranchId;
            order.State = state;
            _context.SaveChanges();
            return RedirectToAction("OrderList", new { id = order.Branch.BranchId });
            //9384883023
        }
        #region
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
        #endregion
        public async Task<IActionResult> Profile(string id)//.SingleOrDefault(x => x.Id == id)
        {
           
            var branchmanager = await _context.BranchManager.Include(b => b.Branch).ThenInclude(r => r.Restaurant).SingleOrDefaultAsync(x => x.Id == id);
            ViewBag.restaurant = branchmanager.Branch.Restaurant;
            return View(branchmanager);
        }

        #region updet from admin
        // POST: /BranchManager/UpdateInfo
    //    [HttpPost]
    //public IActionResult UpdateInfo(BranchManager manager)
    //{
    //    var branchManager = _context.BranchManager.SingleOrDefault(x => x.Id == manager.Id);

    //    if (branchManager != null)
    //    {
    //        //branchManager.Username = manager.Username;
    //        //branchManager.Password = manager.Password;

    //        _context.SaveChanges();
    //        ViewBag.NotificationMessage = "Account details updated successfully!";
    //        ViewBag.NotificationType = "success";
    //    }
    //    else
    //    {
    //        ViewBag.NotificationMessage = "Branch manager not found!";
    //        ViewBag.NotificationType = "error";
    //    }

    //    return RedirectToAction("Profile", new { id = manager.Id });
    //}

    //// POST: /BranchManager/UpdateBranch
    //[HttpPost]
    //public IActionResult UpdateBranch(int id, Branch branch)
    //{
    //    var existingBranch = _context.Branch.Include(b => b.Restaurant).SingleOrDefault(x => x.BranchId == id);

    //    if (existingBranch != null)
    //    {
    //        //existingBranch.BranchArea = branch.BranchArea;
    //        //existingBranch.BranchLocation = branch.BranchLocation;
    //        existingBranch.OpeningHour = branch.OpeningHour;
    //        existingBranch.ClosingHour = branch.ClosingHour;

    //        _context.SaveChanges();
    //        ViewBag.NotificationMessage = "Branch details updated successfully!";
    //        ViewBag.NotificationType = "success";
    //    }
    //    else
    //    {
    //        ViewBag.NotificationMessage = "Branch not found!";
    //        ViewBag.NotificationType = "error";
    //    }

    //    return RedirectToAction("Profile", new { id = existingBranch.BranchManager.Id });
    //   }
    //    private int GetCurrentBranchManagerId()
    //    {
    //        //log in??
    //        return 1;
    //    }

        #endregion

    }
}
