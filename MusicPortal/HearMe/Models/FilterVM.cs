using HearMe.BLL.DTM;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HearMe.Models
{
    public class FilterVM
    {
        public FilterVM(List<GenreDTM> genres, int genre, string song)
        {
            genres.Insert(0, new GenreDTM { Name = "All", Id = 0 });
            Genres = new SelectList(genres, "Id", "Name", genre);
            SelectedGenre = genre;
            EnteredSong = song;
        }
        public SelectList Genres { get; }
        public int SelectedGenre { get; }
        public string EnteredSong { get; }
    }
}
