using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyMusicPortal.Models
{
    [Table("tblGenres")]
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [NotNull]
        public string? Name { get; set; }

        public virtual ICollection<Song>? Songs { get; set; }

        public Genre() => Songs = new List<Song>();
    }
}
