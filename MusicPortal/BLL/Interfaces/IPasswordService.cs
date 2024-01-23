using HearMe.BLL.DTM;
using HearMe.DAL.Entities;

namespace HearMe.BLL.Interfaces
{
   public interface IPasswordService
   {
      Task<User> CreatePassword(User item, string? passwordToHash);

      Task<UserToConfirm> CreatePassword(UserToConfirm item, string? passwordToHash);

      Task<bool> IsPasswordCorrect(UserDTM user, string? passwordToCompare);
   }
}
