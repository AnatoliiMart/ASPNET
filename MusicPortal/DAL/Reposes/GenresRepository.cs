using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HearMe.DAL.Reposes
{
   public class GenresRepository : IRepository<Genre>
   {
      private readonly MyDbContext _context;

      public GenresRepository(MyDbContext context) => _context = context;

      public async Task Create(Genre item) => await _context.Genres.AddAsync(item);

      public async Task Delete(int id)
      {
         Genre? genre = await _context.Genres.FindAsync(id);
         if (genre != null)
            _context.Genres.Remove(genre);
      }

      public async Task<Genre?> Get(int id) => await _context.Genres.FindAsync(id);

      public async Task<Genre?> Get(string name) =>
         await _context.Genres.Where(gen => gen.Name == name).FirstOrDefaultAsync();

      public async Task<IEnumerable<Genre>> GetAll() => await _context.Genres.ToListAsync();

      public void Update(Genre item) => _context.Genres.Entry(item).State = EntityState.Modified;
   }
}
