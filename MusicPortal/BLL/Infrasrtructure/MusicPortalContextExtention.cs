using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearMe.BLL.Infrasrtructure
{
    public static class MusicPortalContextExtention
    {
        public static void AddMusicPortalContext(this IServiceCollection services, string connectionString) => 
            services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(connectionString));
    }
}
