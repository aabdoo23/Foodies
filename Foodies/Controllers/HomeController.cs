using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<BaseUser> _baseUser;

        public HomeController(FoodiesDbContext context, ILogger<HomeController> logger, UserManager<BaseUser> baseUser)
        {
            _context = context;
            _logger = logger;
            _baseUser = baseUser;
        }

        public IActionResult CustomerView()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> AdminProfile(Admin adm)
        {

            var restaurant = await _context.Restaurant.Include(x => x.MenuItems).SingleOrDefaultAsync(x => x.Id == adm.RestaurantId);
            ViewBag.Rest = restaurant;  // Assign restaurant to ViewBag.Rest

            ViewBag.Branch = _context.Branch.Where(x => x.Restaurant == restaurant).ToList();


            ViewBag.menu = _context.MenuItem.Where(x => x.Resturant.Id == adm.RestaurantId).ToList();
            return View(adm);
        }
        public IActionResult Addmnuitm(int id)
        {
            // Get the restaurant by id
            var restu = _context.Restaurant.FirstOrDefault(x => x.Id == id);

            return View(restu); // Pass the restaurant to the view
        }
        public async Task<IActionResult> SaveAddmnu(MenuItem Menu, int restaurantId)
        {
            MenuItem mnu = new MenuItem
            {
                Name = Menu.Name,
                Category = Menu.Category,
                Description = Menu.Description,
                Resturant = await _context.Restaurant.FindAsync(restaurantId)
            };

            await _context.AddAsync(mnu);
            await _context.SaveChangesAsync();

            var adm = await _context.Admin.SingleOrDefaultAsync(x => x.RestaurantId == mnu.Resturant.Id);
            return RedirectToAction("AdminProfile", adm);
        }
        public async Task<IActionResult> Editemnuitm(int id)
        {
            var item = await _context.MenuItem.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        public async Task<IActionResult> SaveEdit(MenuItem Menu)
        {
            var mnu = await _context.MenuItem.Include(x => x.Resturant).SingleOrDefaultAsync(x => x.Id == Menu.Id);
            if (mnu != null)
            {
                mnu.Name = Menu.Name;
                mnu.Price = Menu.Price;
                mnu.Category = Menu.Category;
                mnu.Description = Menu.Description;
                await _context.SaveChangesAsync();
                var adm = await _context.Admin.SingleOrDefaultAsync(x => x.RestaurantId == mnu.Resturant.Id);
                return RedirectToAction("AdminProfile", adm);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> Deletitem(int id)
        {
            var item = await _context.MenuItem.Include(x => x.Resturant).SingleOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _context.MenuItem.Remove(item);
                await _context.SaveChangesAsync();
                var adm = await _context.Admin.SingleOrDefaultAsync(x => x.RestaurantId == item.Resturant.Id);
                return RedirectToAction("AdminProfile", adm);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> UserView(string id)
        {
            var Cus = await _baseUser.FindByIdAsync(id);
            return View(Cus);
        }
        public async Task<IActionResult> AccountInfo(Customer Cus)
        {
            await _baseUser.UpdateAsync(Cus);
            ViewBag.NotificationMessage = "Customer Updated successfully!";
            ViewBag.NotificationType = "success";
            return RedirectToAction("User", Cus);
        }
        public async Task<IActionResult> AddMenuItem(MenuItem Menu)
        {
            await _context.MenuItem.AddAsync(Menu);
            await _context.SaveChangesAsync();
            return RedirectToAction("AdminProfile");

        }
        public async Task<IActionResult> CusAddress(Customer Cus)
        {
            await _baseUser.UpdateAsync(Cus);
            return RedirectToAction("User", Cus);
        }

        /*  public IActionResult AddBranch(int id) { 
           var Admin = _context.Admin.SingleOrDefault(x=>x.RestaurantId == id);
               ViewBag.RestId = id;
              return View(Admin);
          }
          public IActionResult SaveBranch(Branch brnch,BranchManager BrMngr) {
            Branch Newbranch = new Branch();
            BranchManager Newbranchmngr = new BranchManager();
              Newbranch.Restaurant= brnch.Restaurant;
              Newbranch.BranchArea = brnch.BranchArea;
              Newbranch.BranchLocation = brnch.BranchLocation;
              Newbranch.OpeningHour = brnch.OpeningHour;
              Newbranch.ClosingHour = brnch.ClosingHour;
              _context.Add(Newbranch);
              _context.SaveChanges();
              Newbranchmngr.Username= BrMngr.Username;
              Newbranchmngr.Password = BrMngr.Password;
              Newbranchmngr.Admin = BrMngr.Admin;
              Newbranchmngr.BranchId = 7;
              _context.Add(Newbranchmngr);
              _context.SaveChanges();
              return RedirectToAction("AddBranch");
          }*/
        public async Task<IActionResult> AddBranch(int id)
        {
            var admin = await _context.Admin.FirstOrDefaultAsync(x => x.RestaurantId == id);
            ViewBag.RestId = id;
            return View(admin);
        }
        public async Task<IActionResult> SaveBranch(Branch brnch, BranchManager BrMngr, int restaurantId, string adminId)
        {
            brnch.Restaurant = await _context.Restaurant.FindAsync(restaurantId);
            await _context.Branch.AddAsync(brnch);
            await _context.SaveChangesAsync();
            var admin = await _baseUser.FindByIdAsync(adminId);//good
            //TODO: Check mapping
            BrMngr.BranchId = brnch.BranchId;
            BrMngr.Admin = (Admin)admin;

            await _baseUser.CreateAsync(BrMngr);

            return RedirectToAction("AdminProfile", admin);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
