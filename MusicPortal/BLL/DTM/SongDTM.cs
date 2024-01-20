using HearMe.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearMe.BLL.DTM
{
   public class SongDTM
   {
      public int Id { get; set; }

      public string? Name { get; set; }

      public string? SongPath { get; set; }

      public string? PreviewPath { get; set; }

      public int Rating { get; set; } = 0;

      public int? UserId { get; set; }

      public int? GenreId { get; set; }

      public string? GenreName { get; set; }

      public string? UserLogin { get; set; }

   }
}
