using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearMe.DAL.Entities
{
   [Table("tblGenres")]
   public class Genre
   {
      [Key]
      public int Id { get; set; }

      public string? Name { get; set; }

      public virtual ICollection<Song> Songs { get; set; }

      public Genre() => Songs = new List<Song>();
   }
}
