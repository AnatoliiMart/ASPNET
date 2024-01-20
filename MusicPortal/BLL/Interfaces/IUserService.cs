using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearMe.BLL.DTM;

namespace HearMe.BLL.Interfaces
{
   public interface IUserService
   {
      Task CreateUser(UserDTM user);
      Task<IEnumerable<UserDTM>> GetUsersList();
      Task DeleteUser(int id);
      Task<UserDTM> GetUser(int id);
      Task UpdateUser(UserDTM user);
   }
}
