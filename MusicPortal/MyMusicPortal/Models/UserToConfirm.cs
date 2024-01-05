using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyMusicPortal.Models
{
    public class UserToConfirm
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
    }
}
