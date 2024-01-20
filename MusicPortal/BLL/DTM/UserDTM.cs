namespace HearMe.BLL.DTM
{
   public class UserDTM
   {
      public int Id { get; set; }

      public string? FirstName { get; set; }

      public string? LastName { get; set; }

      public string? Login { get; set; }

      public string? AvatarPath { get; set; }

      public string? Password { get; set; }

      public string? Salt { get; set; }

      public bool IsAdmin { get; set; } = false;
   }
}
