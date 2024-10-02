using Foodies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Foodies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<FoodiesDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            
            builder.Services.AddIdentity<BaseUser,IdentityRole>().AddEntityFrameworkStores<FoodiesDbContext>();
            builder.Services.AddIdentityCore<Customer>().AddEntityFrameworkStores<FoodiesDbContext>();
            builder.Services.AddIdentityCore<Admin>().AddEntityFrameworkStores<FoodiesDbContext>();
            builder.Services.AddIdentityCore<BranchManager>().AddEntityFrameworkStores<FoodiesDbContext>();


            var app = builder.Build();

            app.UseHttpsRedirection();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
                
            });

            //app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",


                pattern: "{controller=Home}/{action=CustomerView}");


            app.Run();
        }
    }
}
