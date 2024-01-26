using System.ComponentModel.DataAnnotations;

namespace HearMe.BLL.DTM
{
    public class UserDTM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ("First name field is required"))]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name field is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Login field is required")]
        public string? Login { get; set; }

        public string? AvatarPath { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;
    }
}
