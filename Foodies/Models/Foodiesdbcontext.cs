using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Metadata;

namespace Foodies.Models
{
    public class Foodiesdbcontext : DbContext
    {
        public Foodiesdbcontext() { }

        public Foodiesdbcontext(DbContextOptions<Foodiesdbcontext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseSqlServer("Server=.;Database=tes;User Id=Admin;Password=Admin_123;Integrated Security=True;Encrypt=False");

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
             //One-to-one relationship
             modelBuilder.Entity<Order>()
             .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Order>(o => o.PaymentId);


            //many to many 
            modelBuilder.Entity<Order>().HasAlternateKey(o => o.OrderId);

            modelBuilder.Entity<MenuItem>().HasAlternateKey(m => m.MenuItemId);



            //modelBuilder.Entity<Order>()
            //.HasMany(o => o.Items)
            //.WithMany(mi => mi.Orders)
            //.UsingEntity(j => j.ToTable("OrderMenuItems"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Branch> Branchs { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
    }
}

