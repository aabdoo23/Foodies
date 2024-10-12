using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using Foodies.Models;
using Foodies.ViewModels;
using Foodies.ViewModels.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Foodies.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodiesDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ImageUploader _imageUploader;  
        public HomeController(FoodiesDbContext context, ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> baseUser, RoleManager<IdentityRole> roleManager, ImageUploader imageUploader)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = baseUser;
            _signInManager = signInManager;
            _imageUploader = imageUploader;

        }
        public IActionResult CustomerView()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> AdminProfile(string id)
        {
            var Adminmndr = await _userManager.FindByIdAsync(id);
            var Admncon = _context.Admin.Where(x => x.Id == id).FirstOrDefault();
            var restaurant = await _context.Restaurant.Include(x => x.MenuItems).SingleOrDefaultAsync(x => x.Id == Admncon.RestaurantId);
            AdminProfileViewmodel Admine=new AdminProfileViewmodel();
            //Admine.Id = new { id = Adminmndr.Id }
            Admine.Id = Adminmndr.Id;
            Admine.Email = Adminmndr.Email;
            Admine.FirstName= Admncon.FirstName;
            Admine.LastName= Admncon.LastName;
            Admine.PhoneNumber = Adminmndr.PhoneNumber;
            Admine.Resturantid = Admncon.RestaurantId;
            Admine.img = restaurant.Photo;



            ViewBag.Rest = restaurant; 

            ViewBag.Branch = _context.Branch.Where(x => x.Restaurant == restaurant).Include(c => c.Address).ToList();

            ViewBag.menu = _context.MenuItem.Where(x => x.Resturant.Id == Admncon.RestaurantId).ToList();
            return View(Admine);
        }
        public async Task<IActionResult>EditAdmin(AdminProfileViewmodel adm)
        {
            var Adminmndr = await _userManager.FindByIdAsync(adm.Id);
            var Admncon = _context.Admin.Where(x => x.Id == adm.Id).FirstOrDefault();
            Adminmndr.Email = adm.Email;
            Admncon.FirstName = adm.FirstName;
            Admncon.LastName = adm.LastName;
            Adminmndr.PhoneNumber = adm.PhoneNumber;
            _context.SaveChanges();
            var addm = await _context.Admin.SingleOrDefaultAsync(x => x.Id ==adm.Id);

            return RedirectToAction("AdminProfile", addm);

        }
        public async Task<IActionResult> EditRest(RestaurantEditView res)
        {
            var restaurant = await _context.Restaurant.SingleOrDefaultAsync(x=>x.Id == res.Id);
            restaurant.Name= res.Name;
            restaurant.CuisineType=res.Cuisine;
            restaurant.Hotline=res.Hotline;
            _context.SaveChanges();
            var adm = await _context.Admin.SingleOrDefaultAsync(x => x.Id == res.AdminId);
            return RedirectToAction("AdminProfile", adm);
        }
        public IActionResult Addmnuitm(int id)
        {
            var restu = _context.Restaurant.FirstOrDefault(x => x.Id == id);

            return View(restu);
        }
        public async Task<IActionResult> SaveAddmnu(MenuItem Menu, int restaurantId, IFormFile immg)
        {
            string? usrl = await _imageUploader.UploadImageAsync(immg);
            MenuItem mnu = new MenuItem
            {
                Name = Menu.Name,
                Category = Menu.Category,
                Description = Menu.Description,
                Price = Menu.Price,
                img = usrl,
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
                mnu.Resturant = Menu.Resturant;
                await _context.SaveChangesAsync();
                var adm = await _context.Admin.SingleOrDefaultAsync(x => x.RestaurantId == mnu.Resturant.Id);
                return RedirectToAction("AdminProfile", adm);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> DeletAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id); // Find the user by their ID
            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction("view", "Master");//Master/view
        }
        public IActionResult changepass(string id)
        {

            return View("changepass", id);
        }

        public async Task<IActionResult> changepasso(string oldpas, string Newpass, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            SignInResult result = await _signInManager.PasswordSignInAsync(user, oldpas, false, false);
            //if (result.Succeeded)
            //{
            //    user.PasswordHash = Newpass;
            //    _context.SaveChanges();
            //    return RedirectToAction("AdminProfile",user);
            //}
            if (result.Succeeded)
            {
                // Change the password and hash it
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, Newpass);

                if (passwordChangeResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false); 
                    return RedirectToAction("view", "Master");
                }
                else
                {
                    ViewBag.NotificationMessage = "The New Password invalid";
                    ViewBag.NotificationType = "danger";
                    return View("changepass", id);
                }
            }
            ViewBag.NotificationMessage = "Worng old Password";
            ViewBag.NotificationType = "danger";
            return View("changepass",id);
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
        public async Task<IActionResult> UserView()
        {
            var id = _userManager.GetUserId(User);

            var Cususer = await _userManager.FindByIdAsync(id);

            var cutomer=_context.Customer.Where(x=>x.Id==id).FirstOrDefault();  
            var addrress=_context.Address.Where(x=>x.Id==cutomer.AddressId).FirstOrDefault();
            Customerviewmodel cusview = new Customerviewmodel();
            cusview.Id = id;
            cusview.FirstName = cutomer.FirstName;
            cusview.LastName = cutomer.LastName;
            cusview.Email = Cususer.Email;
            cusview.Phone = Cususer.PhoneNumber;
            cusview.City = addrress.City;
            cusview.Street = addrress.Street;
            cusview.bulding = addrress.Building;
            cusview.Points= cutomer.Points;
            cusview.Location = addrress.Location;
            cusview.img= cutomer.img;
            return View(cusview);
        }
        
       public async Task<IActionResult> AccountInfo(Customerviewmodel cus)
        {
            var Cususer = await _userManager.FindByIdAsync(cus.Id);
            var cutomer = _context.Customer.Where(x => x.Id == cus.Id).FirstOrDefault();
            cutomer.FirstName = cus.FirstName;
            cutomer.LastName = cus.LastName;
            Cususer.Email = cus.Email;
            Cususer.PhoneNumber = cus.Phone;
            _context.SaveChanges();
            ViewBag.NotificationMessage = "Customer Updated successfully!";
           ViewBag.NotificationType = "success";
            
           return RedirectToAction("UserView", cus);
        }
        public async Task<IActionResult> AddMenuItem(MenuItem Menu)
        {
            await _context.MenuItem.AddAsync(Menu);
            await _context.SaveChangesAsync();
            return RedirectToAction("AdminProfile");

        }
        public async Task<IActionResult> CusAddress(Customerviewmodel cus)
        {
            var Cususer = await _userManager.FindByIdAsync(cus.Id);
            var cutomer = _context.Customer.Where(x => x.Id == cus.Id).FirstOrDefault();
            var addrress = _context.Address.Where(x => x.Id == cutomer.AddressId).FirstOrDefault();
            addrress.City = addrress.City;

            addrress.Street = cus.Street;
            addrress.Building = cus.bulding;
            addrress.Location = cus.Location;
            _context.SaveChanges();
            return RedirectToAction("UserView", cus);
        }

        /*  public iactionresult addbranch(int id) { 
           var admin = _context.admin.singleordefault(x=>x.restaurantid == id);
               viewbag.restid = id;
              return view(admin);
          }
          public iactionresult savebranch(branch brnch,branchmanager brmngr) {
            branch newbranch = new branch();
            branchmanager newbranchmngr = new branchmanager();
              newbranch.restaurant= brnch.restaurant;
              newbranch.brancharea = brnch.brancharea;
              newbranch.branchlocation = brnch.branchlocation;
              newbranch.openinghour = brnch.openinghour;
              newbranch.closinghour = brnch.closinghour;
              _context.add(newbranch);
              _context.savechanges();
              newbranchmngr.username= brmngr.username;
              newbranchmngr.password = brmngr.password;
              newbranchmngr.admin = brmngr.admin;
              newbranchmngr.branchid = 7;
              _context.add(newbranchmngr);
              _context.savechanges();
              return redirecttoaction("addbranch");
          }*/
        [HttpGet]
        public IActionResult AddBranch(int id)
        {
            var admin = _context.Admin.SingleOrDefault(x => x.RestaurantId == id);
            //ViewBag.RestId = id;
            Response.Cookies.Append("resiid", id.ToString());
            //int resId = int.Parse(Request.Cookies["resiid"]);

            return View(admin);
            //return Content($"done done done{resId}");

        }

        [HttpPost]
        public async Task<IActionResult> SaveBranch( AddbrancViewmodel adbr)//Branch brnch, BranchManager BrMngr, int restaurantId, int adminId
        {
            IdentityUser user = new IdentityUser();
            user.UserName = adbr.Email;
            user.Email = adbr.Email;
            user.PhoneNumber = adbr.phoneNumber;

            int resId = int.Parse(Request.Cookies["resiid"]);

            // restaurant 
            Restaurant res = _context.Restaurant.Where(x => x.Id == resId).
                Include(c => c.Branches).FirstOrDefault();


            IdentityResult result = await _userManager.CreateAsync(user, adbr.Password);

            Branch newBranch = new Branch
            {
                Restaurant = res,
                OpeningHour = adbr.OpeningHour,
                ClosingHour = adbr.ClosingHour,
                Address = new Address
                {
                    City = adbr.City,
                    Street = adbr.Street,
                    Building = adbr.Building,
                    Location = adbr.Location,
                }
            };

            //return Content($"done done done{resId}");

            res.Branches.Add(newBranch);  


            
            _context.Branch.Add(newBranch);
            _context.SaveChanges();

            //logged in user
            //var userId = _userManager.GetUserId(User);
            var admin = _context.Admin.SingleOrDefault(x => x.RestaurantId == resId);

            BranchManager newBranchMngr = new BranchManager  //adminID
            {
                Id= user.Id,
                AdminId = admin.Id,
                BranchId = newBranch.BranchId,
                FirstName= adbr.FirstName,  
                LastName= adbr.LastName,
                IdentityUser = user,


            };
            _context.BranchManager.Add(newBranchMngr);
            _context.SaveChanges();
            //return C
            return RedirectToAction("AdminProfile", admin);
        }


    }
}
