using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;
using Foodies.Interfaces.Services;
using Foodies.Models;
using Foodies.Repositories;
using Foodies.ViewModels;
using Foodies.ViewModels.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminRepository _adminRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IBranchManagerService _branchManagerService;
        public HomeController(ILogger<HomeController> logger,
            IAdminRepository adminRepository,
            IRestaurantRepository restaurantRepository,
            IBranchRepository branchRepository,
            IMenuItemRepository menuItemRepository,
            ICustomerRepository customerRepository,
            IAddressRepository addressRepository,
            IBranchManagerService branchManagerService)
        {
            _logger = logger;
            _adminRepository = adminRepository;
            _restaurantRepository = restaurantRepository;
            _branchRepository = branchRepository;
            _menuItemRepository = menuItemRepository;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _branchManagerService = branchManagerService;
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
            var admin= await _adminRepository.GetById(id);
            var restaurant = await _restaurantRepository.GetByIdWithMenuItems(admin.RestaurantId);
            AdminProfileViewmodel adminProfileViewModel = new AdminProfileViewmodel();
            //Admine.Id = new { id = Adminmndr.Id }
            adminProfileViewModel.Id = admin.Id;
            adminProfileViewModel.Email = admin.IdentityUser.Email;
            adminProfileViewModel.FirstName = admin.FirstName;
            adminProfileViewModel.LastName = admin.LastName;
            adminProfileViewModel.Phone = admin.IdentityUser.PhoneNumber;
            adminProfileViewModel.Resturant = restaurant;
            adminProfileViewModel.Branch = await _branchRepository.GetAllBrancheshByRestaurantId(admin.RestaurantId);

            return View(adminProfileViewModel);
        }
        public async Task<IActionResult> EditAdmin(AdminProfileViewmodel adm)
        {
            var admin = await _adminRepository.GetById(adm.Id);

            admin.IdentityUser.Email = adm.Email;
            admin.FirstName = adm.FirstName;
            admin.LastName = adm.LastName;
            admin.IdentityUser.PhoneNumber = adm.PhoneNumber;
            await _adminRepository.Update(admin);

            //TODO: Check if the identity user is updated

            return RedirectToAction("AdminProfile", adm);

        }
        public async Task<IActionResult> EditRest(RestaurantEditView res)
        {
            var restaurant = await _restaurantRepository.GetById(res.Id);
            restaurant.Name = res.Name;
            restaurant.CuisineType = res.Cuisine;
            restaurant.Hotline = res.Hotline;
            await _restaurantRepository.Update(restaurant);
            var admin = await _adminRepository.GetById(res.AdminId);
            return RedirectToAction("AdminProfile", admin);
        }
        [HttpGet]
        public async Task<IActionResult> AddMenuItem(string id)
        {
            var restaurant = await _restaurantRepository.GetById(id);
            return View(restaurant);
        }
        public async Task<IActionResult> SaveAddmnu(MenuItem Menu, string restaurantId)
        {
            MenuItem menuItem = new MenuItem
            {
                Name = Menu.Name,
                Category = Menu.Category,
                Description = Menu.Description,
                Resturant = await _restaurantRepository.GetById(restaurantId),
            };
            await _menuItemRepository.Create(menuItem);

            var admin = await _adminRepository.GetAdminByRestaurantId(restaurantId);
            return RedirectToAction("AdminProfile", admin);
        }
        public async Task<IActionResult> EditMenuItem(string id)
        {
            var item = await _menuItemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        public async Task<IActionResult> SaveEdit(MenuItem itemForEdit)
        {
            var menuItem = await _menuItemRepository.GetById(itemForEdit.Id);
            if (menuItem != null)
            {
                menuItem.Name = itemForEdit.Name;
                menuItem.Price = itemForEdit.Price;
                menuItem.Category = itemForEdit.Category;
                menuItem.Description = itemForEdit.Description;

                await _menuItemRepository.Update(menuItem);
                var admin = await _adminRepository.GetAdminByRestaurantId(menuItem.Resturant.Id);
                return RedirectToAction("AdminProfile", admin);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public async Task<IActionResult> DeleteMenuItem(string id)
        {
            var menuItem = await _menuItemRepository.GetById(id);
            await _menuItemRepository.Delete(id);
            var admin = await _adminRepository.GetAdminByRestaurantId(menuItem.Resturant.Id);
            return RedirectToAction("AdminProfile", admin);
        }
        public async Task<IActionResult> UserView(string id)
        {
            var customer = await _customerRepository.GetById(id);
            var customerViewModel = new Customerviewmodel();

            customerViewModel.Id = id;
            customerViewModel.FirstName = customer.FirstName;
            customerViewModel.LastName = customer.LastName;
            customerViewModel.Email = customer.IdentityUser.Email;
            customerViewModel.Phone = customer.IdentityUser.PhoneNumber;
            customerViewModel.City = customer.Address.City;
            customerViewModel.Street = customer.Address.Street;
            customerViewModel.bulding = customer.Address.Building;
            customerViewModel.Points = customer.Points;
            customerViewModel.Location = customer.Address.Location;

            return View(customerViewModel);
        }

        public async Task<IActionResult> AccountInfo(Customerviewmodel cus)
        {
            var customer = await _customerRepository.GetById(cus.Id);

            customer.FirstName = cus.FirstName;
            customer.LastName = cus.LastName;
            customer.IdentityUser.Email = cus.Email;
            customer.IdentityUser.PhoneNumber = cus.Phone;

            await _customerRepository.Update(customer);

            ViewBag.NotificationMessage = "Customer Updated successfully!";
            ViewBag.NotificationType = "success";

            return RedirectToAction("UserView", cus);
        }
        [HttpPost]
        public async Task<IActionResult> AddMenuItem(MenuItem Menu)
        {
            await _menuItemRepository.Create(Menu);
            return RedirectToAction("AdminProfile");

        }
        public async Task<IActionResult> CusAddress(Customerviewmodel cus)
        {
            var customer = await _customerRepository.GetById(cus.Id);

            customer.Address.City = cus.City;
            customer.Address.Street = cus.Street;
            customer.Address.Building = cus.bulding;
            customer.Address.Location = cus.Location;

            await _addressRepository.Update(customer.Address);

            return RedirectToAction("UserView", cus);
        }

        [HttpGet]
        public async Task<IActionResult> AddBranch(string id)
        {
            var admin = await _adminRepository.GetAdminByRestaurantId(id);
            Response.Cookies.Append("resiid", id.ToString());

            return View(admin);

        }

        [HttpPost]
        public async Task<IActionResult> SaveBranch(AddbranchViewmodel adbr)//Branch brnch, BranchManager BrMngr, int restaurantId, int adminId
        {
            //TODO: Check if data not null ffs

            // restaurant 

            string resId = Request.Cookies["resiid"] ?? throw new NotFoundException($"No restaurant of cookie {Request.Cookies["resiid"]} id found");
            Restaurant res = await _restaurantRepository.GetByIdWithMenuItemsAndBranches(resId);

            Branch newBranch = new Branch
            {
                Restaurant = res,
                OpeningHour = adbr.OpeningHour,
                ClosingHour = adbr.ClosingHour,
                Address = new Address
                {
                    Id = Guid.NewGuid().ToString(),
                    City = adbr.City,
                    Street = adbr.Street,
                    Building = adbr.Building,
                    Location = adbr.Location,
                }
            };

            res.Branches.Add(newBranch);

            await _branchRepository.Create(newBranch);

            var admin = await _adminRepository.GetAdminByRestaurantId(resId);

            await _branchManagerService.CreateBranchManager(adbr,admin, newBranch);

            return RedirectToAction("AdminProfile", admin);
        }


    }
}
