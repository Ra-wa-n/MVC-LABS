using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data.Data;
using Project.Data.Model;
using Project.Data.Repo;

namespace Day1_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
          //  builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                s =>
                {
                    s.Password.RequireDigit = false;
                    s.Password.RequireNonAlphanumeric = false;
                    s.Password.RequireUppercase = false;
                    s.Password.RequireLowercase = false;
                    s.SignIn.RequireConfirmedEmail = false;
                }
                ).AddEntityFrameworkStores<PContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IEntityRepo<Student>, StdRepo>();
            //builder.Services.AddScoped<DeptRepo, DeptRepo>();
            //builder.Services.AddScoped<StsCrsRepo, StsCrsRepo>();
            //builder.Services.AddScoped<IEntityRepo<Course>, CrsRepo>();
            builder.Services.AddDbContext<PContext>(s =>
            {
                s.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
