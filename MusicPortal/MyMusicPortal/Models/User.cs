using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyMusicPortal.Models
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [NotNull]
        public string? Name { get; set; }

        [NotNull]
        public string? Surname { get; set; }

        [NotNull]
        public string? Login { get; set; }

        [NotNull]
        public string? Password { get; set; }

        [NotNull]
        public string? Salt { get; set; }

        public virtual ICollection<Song>? Songs { get; set; }

        public User() => Songs = new List<Song>();

    }
}
