using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.BLL.Services;
using Microsoft.EntityFrameworkCore;

namespace HearMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddMusicPortalContext(connection);

            builder.Services.AddControllersWithViews();
            builder.Services.AddUnitOfWorkService();
            builder.Services.AddTransient<IModelService<SongDTM>, SongService>();
            builder.Services.AddTransient<IModelService<GenreDTM>, GenreService>();
            builder.Services.AddTransient<IModelService<UserDTM>, UserService>();
            builder.Services.AddTransient<IPasswordService, PasswordService>();
            builder.Services.AddTransient<IUserToConfirmService, UserToConfirmService>();

            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}");

            app.Run();
        }
    }
}
