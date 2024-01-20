using HearMe.BLL.DTM;

namespace HearMe.BLL.Interfaces
{
   public interface ISongService
   {
      Task CreateSong(SongDTM song);
      Task UpdateSong(SongDTM song);
      Task<SongDTM> GetSong(int id);
      Task<IEnumerable<SongDTM>> GetSongsList();
      Task DeleteSong(int id);
   }
}
