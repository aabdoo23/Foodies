using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Foodies.Models
{
	public class Foodiesdbcontext: DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=.;Database=Foodestest1;User Id=Admin;Password=Admin_123;Integrated Security=True;Encrypt=False");
		}
		
		public virtual DbSet<Admin> Admins { get; set; }
		public virtual DbSet<Branch> Branchs { get; set; }
		public virtual DbSet<Rating> Ratings { get; set; }
		public virtual DbSet<Cart> Carts { get; set; }
		public virtual DbSet<MenuItem> MenuItems { get; set; }
		public virtual DbSet<Restaurant> Restaurants { get; set; }
		public virtual DbSet<Payment> Payments { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
	}
}
