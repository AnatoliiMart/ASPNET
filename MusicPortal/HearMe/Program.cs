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

         string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
         builder.Services.AddMusicPortalContext(connection);

         builder.Services.AddControllersWithViews();
         builder.Services.AddUnitOfWorkService();
         builder.Services.AddTransient<IModelService<SongDTM>, SongService>();
         builder.Services.AddTransient<IModelService<GenreDTM>, GenreService>();
         builder.Services.AddTransient<IModelService<UserDTM>, UserService>();
         builder.Services.AddTransient<IUserToConfirmService, UserToConfirmService>();

         var app = builder.Build();

         app.UseStaticFiles();

         app.MapControllerRoute(
             name: "default",
             pattern: "{controller=Home}/{action=Index}/{id?}");

         app.Run();
      }
   }
}
