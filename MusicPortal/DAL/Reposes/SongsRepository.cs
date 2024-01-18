using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HearMe.DAL.Reposes
{
   public class SongsRepository : IRepository<Song>
   {
      private readonly MyDbContext _context;

      public SongsRepository(MyDbContext context) => _context = context;

      public async Task Create(Song item) => await _context.Songs.AddAsync(item);

      public async Task Delete(int id)
      {
         Song? song = await _context.Songs.FindAsync(id);
         if (song != null)
            _context.Songs.Remove(song);
      }

      public async Task<Song?> Get(int id) =>
         await _context.Songs.Include(s => s.Genre).Include(s => s.User)
               .Where(s => s.Id == id).FirstOrDefaultAsync();

      public async Task<Song?> Get(string name) =>
         await _context.Songs.Include(s => s.Genre).Include(s => s.User)
               .Where(s => s.Name == name).FirstOrDefaultAsync();

      public async Task<IEnumerable<Song>> GetAll() =>
         await _context.Songs.Include(s => s.Genre).Include(s => s.User).ToListAsync();

      public void Update(Song item) => _context.Songs.Entry(item).State = EntityState.Modified;
   }
}
