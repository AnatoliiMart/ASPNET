using System.ComponentModel;
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
        
        [DisplayName("User")]
        public int UserId { get; set; }

        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [NotNull]
        public string? Path { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set;}

        [ForeignKey("GenreId")]
        public virtual Genre? Genre { get; set; }
    }
}
