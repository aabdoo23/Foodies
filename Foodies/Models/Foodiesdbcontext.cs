namespace Foodies.Models
{
    public class FoodiesDbContext : DbContext
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

            //many to many 
            modelBuilder.Entity<Order>().HasAlternateKey(o => o.Id);

            modelBuilder.Entity<MenuItem>().HasAlternateKey(m => m.Id);


            //modelBuilder.Entity<Order>()
            //.HasMany(o => o.Items)
            //.WithMany(mi => mi.Orders)
            //.UsingEntity(j => j.ToTable("OrderMenuItems"));

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<BranchManager> BranchManager { get; set; }
    }
}

