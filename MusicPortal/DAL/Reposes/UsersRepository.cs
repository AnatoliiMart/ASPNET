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
   public class UsersRepository : IRepository<User>
   {
      private readonly MyDbContext _context;

      public UsersRepository(MyDbContext context) => _context = context;

      public Task Create(User item)
      {
         throw new NotImplementedException();
      }

      public Task Delete(int id)
      {
         throw new NotImplementedException();
      }

      public Task<User> Get(int id)
      {
         throw new NotImplementedException();
      }

      public Task<User> Get(string name)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<User>> GetAll()
      {
         throw new NotImplementedException();
      }

      public void Update(User item)
      {
         throw new NotImplementedException();
      }
   }
}
