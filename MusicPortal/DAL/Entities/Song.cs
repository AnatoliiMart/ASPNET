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
    public class Song
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? SongPath { get; set; }

        public string? PreviewPath { get; set; }

        public int Rating { get; set; }

        public int? UserId { get; set; }

        public int? GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre? Genre { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }


    }
}
