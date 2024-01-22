using HearMe.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearMe.BLL.DTM
{
   public class SongDTM
   {
      public int Id { get; set; }

      [Required(ErrorMessage = "Name field is reqired")]
      public string? Name { get; set; }

      [Required]
      public string? SongPath { get; set; }

      [Required]
      public string? PreviewPath { get; set; }

      [Required]
      public int Rating { get; set; } = 0;

      public int? UserId { get; set; }

      public int? GenreId { get; set; }

      [Required]
      public string? GenreName { get; set; }

      [Required]
      public string? UserLogin { get; set; }

   }
}
