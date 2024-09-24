using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
namespace Foodies.Controllers
{
	public class CustomerViewController : Controller
	{
		Foodiesdbcontext context=new Foodiesdbcontext();
		public IActionResult index()
		{
		var mnui=context.Restaurant.ToList();
		  return View(mnui);
		}
		public IActionResult Minew(int id) {

			var rest = context.Restaurant.Where( x => x.Id == id).SingleOrDefault();
            ViewData["data"]= context.MenuItem.Where(x => x.Resturant.Id == id).ToList();
           // var result=context.MenuItems.Where(x=>x.Resturant.RestaurantId==id).ToList();
            return View(rest);

		}

       


    }
}

