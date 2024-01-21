using HearMe.BLL.DTM;

namespace HearMe.BLL.Interfaces
{
   public interface IPasswordCreation<T> where T : class
   {
      Task<UserDTM> CreatePassword(UserDTM item, string? passwordToHash);

      Task<bool> IsPasswordCorrect(T user, string? passwordToCompare);
   }
}
