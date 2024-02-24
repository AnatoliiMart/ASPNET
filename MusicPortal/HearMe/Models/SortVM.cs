namespace HearMe.Models
{
    public class SortVM
    {
        public SortVM(SortState sortOrder)
        {
            //defaults
            SongNameSort = SortState.SongNameAsc;
            GenreNameSort = SortState.SongNameAsc;
            UserLoginSort = SortState.SongNameAsc;

            SongNameSort = sortOrder == SortState.SongNameAsc ? SortState.SongNameDesc : SortState.SongNameAsc;
            GenreNameSort = sortOrder == SortState.SongGenreAsc ? SortState.SongGenreDesc : SortState.SongGenreAsc;
            UserLoginSort = sortOrder == SortState.UserAsc ? SortState.UserDesc : SortState.UserAsc;
            Current = sortOrder;
        }

        public SortState SongNameSort { get; set; }
        public SortState GenreNameSort { get; set; }
        public SortState UserLoginSort { get; set; }
        public SortState Current { get; set; }
        public bool Up { get; set; }
    }
}
