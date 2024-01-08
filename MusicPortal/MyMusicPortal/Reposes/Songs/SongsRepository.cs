using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes.Songs
{
    public class SongsRepository : ISongsRepository
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SongsRepository(MyDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Song?> GetSongById(int? id) =>
            await _context.Songs
                .Include(s => s.Genre)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Song>> GetSongsList() =>
             await _context.Songs.Include(s => s.Genre).Include(s => s.User).ToListAsync();

        public async Task<bool> IsExtentionCorrect(string ext) =>
            await Task.Run(() =>
                   (ext != "audio/flac" &&
                    ext != "audio/mpeg" &&
                    ext != "audio/wav" &&
                    ext != "audio/aac" &&
                    ext != "audio/m4a" &&
                    ext != "audio/ogg" &&
                    ext != "audio/mid"));
        

        public async Task<bool> IsSongUnique(Song song) =>
            await _context.Songs.AnyAsync(_song => _song.Name == song.Name && _song.Id != song.Id);
        

        public async Task SongUpload(Song song, IFormFile fileUpload)
        {
            string path = "/songs/" + fileUpload.FileName;

            using (FileStream filestream = new(_environment.WebRootPath + path, FileMode.Create))
                await fileUpload.CopyToAsync(filestream);
            song.Path = path;
        }

        public IQueryable<User> GetUserByLogin(string? login) => _context.Users.Where(x => x.Login == login);

        public async Task AddSongToDb(Song song)
        {
            await _context.AddAsync(song);
            await Save();
        }
        public async Task<List<Genre>> GetGenresList() => await _context.Genres.ToListAsync();
        
        private async Task Save() => await _context.SaveChangesAsync();
    }
}
