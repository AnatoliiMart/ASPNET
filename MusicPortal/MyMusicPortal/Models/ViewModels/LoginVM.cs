using System.ComponentModel.DataAnnotations;

namespace MyMusicPortal.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
