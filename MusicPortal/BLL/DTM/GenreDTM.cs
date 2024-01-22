using System.ComponentModel.DataAnnotations;

namespace HearMe.BLL.DTM
{
   public class GenreDTM
   {
      public int Id { get; set; }

      [Required(ErrorMessage = "Name field is required")]
      public string? Name { get; set; }
   }
}
