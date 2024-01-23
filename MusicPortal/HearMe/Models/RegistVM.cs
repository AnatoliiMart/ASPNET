using System.ComponentModel.DataAnnotations;

namespace HearMe.Models
{
   public class RegistVM
   {
      [Required(ErrorMessage ="First name field is required")]
      public string? FirstName { get; set; }

      [Required(ErrorMessage = "Last name field is required")]
      public string? LastName { get; set; }

      [Required(ErrorMessage = "Login field is required")]
      public string? Login { get; set; }

      public string? AvatarPath { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string? Password { get; set; }

      [Required(ErrorMessage = "First name field is required")]
      [DataType(DataType.Password)]
      [Compare("Password", ErrorMessage ="Passwords do not match")]
      public string? ConfirmPassword { get; set; }
   }

}
