using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearMe.BLL.DTM;

namespace HearMe.BLL.Interfaces
{
   public interface IUserToConfirmService: IPasswordCreation
   {
      Task CreateUserToConfirm(UserDTM user);
      Task DeleteUserToConfirm(int id);
      Task<UserDTM> GetUserToConfirm(int id);
      Task<IEnumerable<UserDTM>> GetUsersToConfirmList();
   }
}
