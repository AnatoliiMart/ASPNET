using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models.ViewModels;
using MyMusicPortal.Reposes;

namespace MyMusicPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connection));

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
