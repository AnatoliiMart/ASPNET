using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyMusicPortal.Models
{
    [Table("tblSongs")]
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [NotNull]
        public string? Path { get; set; }

        public virtual User? Users { get; set;}

        public virtual Genre? Genres { get; set; }
    }
}
