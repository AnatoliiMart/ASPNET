using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HearMe.BLL.Infrasrtructure
{
    public static class MusicPortalContextExtention
    {
        public static void AddMusicPortalContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(connectionString));
    }
}
