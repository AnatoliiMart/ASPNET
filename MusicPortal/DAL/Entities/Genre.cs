using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearMe.DAL.Entities
{
    [Table("tblSongs")]
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public Genre() => Songs = new List<Song>();
    }
}
