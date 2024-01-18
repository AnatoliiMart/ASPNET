using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.DAL.Reposes
{
   public class UsersToConfirmRepository : IRepository<UsersToConfirm>
   {
      private readonly MyDbContext _context;

      public UsersToConfirmRepository(MyDbContext context) => _context = context;
      public Task Create(UsersToConfirm item)
      {
         throw new NotImplementedException();
      }

      public Task Delete(int id)
      {
         throw new NotImplementedException();
      }

      public Task<UsersToConfirm> Get(int id)
      {
         throw new NotImplementedException();
      }

      public Task<UsersToConfirm> Get(string name)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<UsersToConfirm>> GetAll()
      {
         throw new NotImplementedException();
      }

      public void Update(UsersToConfirm item)
      {
         throw new NotImplementedException();
      }
   }
}
