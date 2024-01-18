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
   public class GenresRepository : IRepository<Genre>
   {
      private readonly MyDbContext _context;

      public GenresRepository(MyDbContext context) => _context = context;

      public Task Create(Genre item)
      {
         throw new NotImplementedException();
      }

      public Task Delete(int id)
      {
         throw new NotImplementedException();
      }

      public Task<Genre> Get(int id)
      {
         throw new NotImplementedException();
      }

      public Task<Genre> Get(string name)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<Genre>> GetAll()
      {
         throw new NotImplementedException();
      }

      public void Update(Genre item)
      {
         throw new NotImplementedException();
      }
   }
}
