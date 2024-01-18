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
   public class SongsRepository : IRepository<Song>
   {
      private readonly MyDbContext _context;

      public SongsRepository(MyDbContext context) => _context = context;

      public Task Create(Song item)
      {
         throw new NotImplementedException();
      }

      public Task Delete(int id)
      {
         throw new NotImplementedException();
      }

      public Task<Song> Get(int id)
      {
         throw new NotImplementedException();
      }

      public Task<Song> Get(string name)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<Song>> GetAll()
      {
         throw new NotImplementedException();
      }

      public void Update(Song item)
      {
         throw new NotImplementedException();
      }
   }
}
