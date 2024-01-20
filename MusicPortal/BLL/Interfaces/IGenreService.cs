using HearMe.BLL.DTM;

namespace HearMe.BLL.Interfaces
{
   public interface IGenreService
   {
      Task<IEnumerable<GenreDTM>> GetGenresList();
      Task<GenreDTM> GetGenre(int id);
      Task DeleteGenre(int id);
      Task UpdateGenre(GenreDTM genre);
      Task CreateGenre(GenreDTM genre);
   }
}
