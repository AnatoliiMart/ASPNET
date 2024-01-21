using HearMe.DAL.Entities;

namespace HearMe.BLL.Interfaces
{
   public interface IPasswordService
   {
      Task<User> CreatePassword(User item, string? passwordToHash);

      Task<UserToConfirm> CreatePassword(UserToConfirm item, string? passwordToHash);

      Task<bool> IsPasswordCorrect(User user, string? passwordToCompare);
   }
}
