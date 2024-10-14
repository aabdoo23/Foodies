using Foodies.Common;
using Foodies.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

using Microsoft.Extensions.FileProviders;
using Firebase.Storage;
using System.Configuration;
using Foodies.Controllers;


namespace Foodies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllersWithViews();

            //inject custome made repos and services
            builder.Services.AddRepositories();
            builder.Services.AddServiceInjection();

            builder.Services.AddDbContext<FoodiesDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<FoodiesDbContext>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Add Firebase Storage
            var firebaseConfig = builder.Configuration.GetSection("Firebase");
            builder.Services.AddSingleton<FirebaseStorage>(new FirebaseStorage(
                firebaseConfig["Bucket"],
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = async () => null
                }
           ));
            builder.Services.AddTransient<ImageUploader>();

            builder.Services.AddRazorPages();

            builder.Services.AddHttpClient<MapService>();

            builder.Services.AddControllers(); // Ensure you have this

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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",


            //pattern: "{controller=Home}/{action=CustomerView}");
            pattern: "{controller=Master}/{action=view}/{id?}");

            app.Run();
        }
    }
}
