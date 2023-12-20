using GuestsBook.Models;
using GuestsBook.Repos;
using Microsoft.EntityFrameworkCore;

namespace GuestsBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
           
            builder.Services.AddSession();  // Добавляем сервисы сессии

            // Получаем строку подключения из файла конфигурации
            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connection));
            builder.Services.AddScoped<IMyRepository, MyRepository>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseSession();
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
