using ASP_MVC_PR_1.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_PR_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<MovieContext>(options => options.UseSqlServer(connection));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Movies}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
