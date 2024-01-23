using System.ComponentModel.DataAnnotations;

namespace HearMe.Models
{
   public class LoginVM
   {
      [Required(ErrorMessage = "Login field is required")]
      public string? Login { get; set; }

      [Required(ErrorMessage = "Password field is required")]
      [DataType(DataType.Password)]
      public string? Password { get; set; }
   }
}
