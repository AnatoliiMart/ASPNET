using HearMe.BLL.DTM;

namespace HearMe.BLL.Interfaces
{
   public interface IPasswordCreation
   {
      Task<UserDTM> CreatePassword(UserDTM item, string? passwordToHash);
   }
}
