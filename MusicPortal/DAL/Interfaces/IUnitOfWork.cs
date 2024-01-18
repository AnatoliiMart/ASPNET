using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearMe.DAL.Entities;

namespace HearMe.DAL.Interfaces
{
   public interface IUnitOfWork
   {
      IRepository<Song> Songs { get; }

      IRepository<Genre> Genres { get; }

      IRepository<User> Users { get; }

      IRepository<UsersToConfirm> UsersToConfirm { get; }

      Task Save();
   }
}
