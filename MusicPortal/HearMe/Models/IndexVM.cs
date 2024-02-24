using HearMe.BLL.DTM;

namespace HearMe.Models
{
    public class IndexVM
    {
        public IEnumerable<SongDTM> Songs { get; set; }
        public PageVM PageViewModel { get; }
        public FilterVM FilterViewModel { get; }
        public SortVM SortViewModel { get; }

        public IndexVM(IEnumerable<SongDTM> songs, PageVM pageViewModel,
            FilterVM filterViewModel, SortVM sortViewModel)
        {
            Songs = songs;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
