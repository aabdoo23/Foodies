using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Foodies.Models
{
    public class FoodiesDbContext : IdentityDbContext
    {
        public FoodiesDbContext(DbContextOptions<FoodiesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One-to-one relationship
            modelBuilder.Entity<Order>()
            .HasOne(o => o.Payment)
           .WithOne(p => p.Order)
           .HasForeignKey<Order>(o => o.PaymentId);

            //One-to-one relationship
            modelBuilder.Entity<BranchManager>()
            .HasOne(o => o.Branch)
           .WithOne(p => p.BranchManager)
           .HasForeignKey<BranchManager>(o => o.BranchId);

            modelBuilder.Entity<Customer>()
            .HasOne(o => o.Address)
           .WithOne(p => p.Customer)
           .HasForeignKey<Customer>(o => o.AddressId);

            modelBuilder.Entity<Branch>()
            .HasOne(o => o.Address)
           .WithOne(p => p.Branch)
           .HasForeignKey<Branch>(o => o.AddressId);
            //many to many 
            modelBuilder.Entity<Order>().HasAlternateKey(o => o.Id);

            modelBuilder.Entity<MenuItem>().HasAlternateKey(m => m.Id);


            modelBuilder.Entity<BaseUser>()
             .HasOne(o => o.IdentityUser)
            .WithOne()
            .HasForeignKey<BaseUser>(o => o.Id);

            // Configure Admin entity
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admins");  // Map to Admins table
                entity.HasOne(a => a.IdentityUser)
                      .WithOne()  // No navigation property in IdentityUser, so we use WithOne()
                      .HasForeignKey<Admin>(a => a.Id);  // Foreign key to AspNetUsers
            });

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");  // Map to Customers table
                entity.HasOne(c => c.IdentityUser)
                      .WithOne()  // No navigation property in IdentityUser, so we use WithOne()
                      .HasForeignKey<Customer>(c => c.Id);  // Foreign key to AspNetUsers
            });


            // Configure BaseUser without mapping it to a table
            //modelBuilder.Entity<BaseUser>().HasNoKey();  // Exclude BaseUser from the model
            // Ensure BaseUser has a foreign key relationship with IdentityUser
            modelBuilder.Entity<BaseUser>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<BaseUser>(b => b.Id);

            //modelBuilder.Entity<RegisterationViewModel>().HasNoKey();  // Exclude BaseUser from the model
            //modelBuilder.Entity<LogInViewModel>().HasNoKey();  // Exclude BaseUser from the model


            //modelBuilder.Entity<Order>()
            //.HasMany(o => o.Items)
            //.WithMany(mi => mi.Orders)
            //.UsingEntity(j => j.ToTable("OrderMenuItems"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<BranchManager> BranchManager { get; set; }
        public virtual DbSet<Address> Address { get; set; }

    }
}

