using ERP_System.Data.context;

using Microsoft.EntityFrameworkCore;

namespace ERP_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
                options.UseSqlServer(connectionString);
            });


           









            builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyAppCookie";
        options.LoginPath = "/Index";
        options.LogoutPath = "/Admin/Adminlogout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Plans";
    });


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Basic Only", policy =>
                    policy.RequireClaim("Subscription", "Basic Only"));

                options.AddPolicy("Standard Only", policy =>
                    policy.RequireClaim("Subscription", "Standard Only"));

                options.AddPolicy("Advanced Only", policy =>
                    policy.RequireClaim("Subscription", "Advanced Only"));
            });

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true; // Secure cookie
                options.Cookie.IsEssential = true; // Ensure session cookie is set for non-authenticated users
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

 
  
            
            app.UseAuthentication(); // Add authentication middleware

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
