using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes.Genres
{
    public class GenresRepository : IGenresRepository
    {
        private readonly MyDbContext _context;

        public GenresRepository(MyDbContext context) => _context = context;


        public async Task AddGenre(Genre genre)
        {
            _context.Add(genre);
            await Save();
        }

        public async Task DeleteGenre(Genre genre)
        {
            _context.Genres.Remove(genre);
            await Save();
        }
        public async Task UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            await Save();
        }

        public async Task<bool> GenreExists(int id) => await _context.Genres.AnyAsync(e => e.Id == id);
        

        public async Task<Genre?> GetGenre(int? id) => await _context.Genres.FindAsync(id);

        public async Task<Genre?> GetGenreByFoD(int? id) =>
            await _context.Genres.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Genre>> GetGenresList() => await _context.Genres.ToListAsync();

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
