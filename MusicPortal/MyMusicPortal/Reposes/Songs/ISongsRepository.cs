using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes.Songs
{
    public interface ISongsRepository
    {
        Task SongUpload(Song song, IFormFile fileUpload);

        Task<bool> IsExtentionCorrect(string extention);

        Task<bool> IsSongUnique(Song song);

        Task<List<Song>> GetSongsList();

        Task<Song?> GetSongById(int? id);

        IQueryable<User> GetUserByLogin(string? login);

        Task<List<Genre>> GetGenresList();

        Task AddSongToDb(Song song);
    }
}
