using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearMe.DAL.Entities
{
   [Table("tblUsersToConfirm")]
   public class UserToConfirm
   {
      [Key]
      public int Id { get; set; }

      public string? FirstName { get; set; }

      public string? LastName { get; set; }

      public string? Login { get; set; }

      public string? AvatarPath { get; set; }

      public string? Password { get; set; }

      public string? Salt { get; set; }
   }
}
