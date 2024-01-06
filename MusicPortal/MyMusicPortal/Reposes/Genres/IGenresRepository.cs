using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes.Genres
{
    public interface IGenresRepository
    {
        Task<List<Genre>> GetGenresList();

        Task<Genre?> GetGenre(int? id);  

        Task<Genre?> GetGenreByFoD(int? id);

        Task AddGenre(Genre genre);

        Task UpdateGenre(Genre genre);

        Task DeleteGenre(Genre genre);

        Task<bool> GenreExists(int id);

        Task Save();
    }
}
