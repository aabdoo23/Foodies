using Foodies.Data;
using Foodies.Interfaces.Repositories;
using Foodies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class BranchManagerController : Controller
    {
        private readonly ILogger<BranchManagerController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IBranchManagerRepository _branchManagerRepository;
        private readonly IBranchRepository _branchRepository;

        private readonly UserManager<IdentityUser> _usermanager;


        public BranchManagerController(ILogger<BranchManagerController> logger,
            IOrderRepository orderRepository,
            IBranchManagerRepository branchManagerRepository,
            UserManager<IdentityUser> usermanager)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _branchManagerRepository = branchManagerRepository;
            _usermanager = usermanager;
        }

        public async Task<IActionResult> OrderList(string branchId)
        {
            var allOrdersForBranch = await _orderRepository.GetOrdersByBranchIdWithItems(branchId);
            var userid = _usermanager.GetUserId(User);
            var branchmanager = await _branchManagerRepository.GetByIdWithBranchAndRestaurantIncluded(userid);

            ViewBag.b = branchmanager.BranchId;
            return View(allOrdersForBranch);
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
        public async Task<IActionResult> Details(string id)
        {
            var order = await _orderRepository.GetByIdWithBranchIncluded(id);
            
            return View(order);
        }
        public async Task<IActionResult> ChangeState(string id, string state)
        {
            var order = await _orderRepository.GetByIdWithBranchIncluded(id);
            order.State = state;
            await _orderRepository.Update(order);
            return RedirectToAction("OrderList", new { branchId = order.Branch.Id });
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
        public async Task<IActionResult> Profile(string ?id)
        {
            var userid = _usermanager.GetUserId(User);
            var branchmanager = await _branchManagerRepository.GetByIdWithBranchAndRestaurantIncluded(userid);
            ViewBag.add = branchmanager.Branch.Address;
            
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
