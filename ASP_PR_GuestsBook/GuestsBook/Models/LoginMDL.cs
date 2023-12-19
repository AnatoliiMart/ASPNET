using System.ComponentModel.DataAnnotations;

namespace GuestsBook.Models
{
    public class LoginMDL
    {
        [Required]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
