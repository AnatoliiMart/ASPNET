using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearMe.DAL.Entities
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Login { get; set; }

        public string? AvatarPath { get; set; }
         
        public string? Password { get; set; }

        public string? Salt { get; set; }

        public bool IsAdmin { get; set; } = false;

        public virtual ICollection<Song> Songs { get; set; }

        public User() => Songs = new List<Song>();
    }
}
