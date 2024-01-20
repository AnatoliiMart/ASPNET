using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearMe.DAL.Interfaces;
using HearMe.DAL.Reposes;
using Microsoft.Extensions.DependencyInjection;

namespace HearMe.BLL.Infrasrtructure
{
   public static class UnitOfWorkServiceExtention
   {
      public static void AddUnitOfWorkService(this IServiceCollection services) => 
         services.AddScoped<IUnitOfWork, EFUnitOfWork>();
   }
}
