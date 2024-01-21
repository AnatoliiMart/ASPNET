using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.BLL.Services
{
   public class UserToConfirmService : IUserToConfirmService, IPasswordCreation<UserToConfirm>
   {
      public IUnitOfWork DataBase { get; protected set; }

      public UserToConfirmService(IUnitOfWork unit) => DataBase = unit;

      public async Task CreateUserToConfirm(UserDTM user)
      {
         var usr = new UserToConfirm
         {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarPath = user.AvatarPath,
            Login = user.Login,
            Password = user.Password,
            Salt = user.Salt,
         };
         await DataBase.UsersToConfirm.Create(usr);
         await DataBase.Save();
      }

      public async Task DeleteUserToConfirm(int id)
      {
         await DataBase.UsersToConfirm.Delete(id);
         await DataBase.Save();
      }

      public async Task<IEnumerable<UserDTM>> GetUsersToConfirmList()
      {
         var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserToConfirm, UserDTM>()).CreateMapper();
         return mapper.Map<IEnumerable<UserToConfirm>, IEnumerable<UserDTM>>(await DataBase.UsersToConfirm.GetAll());
      }

      public async Task<UserDTM> GetUserToConfirm(int id)
      {
         UserToConfirm? usr = await DataBase.UsersToConfirm.Get(id);
         return usr == null
                ? throw new ValidationException("This User does not found in our base of Users to confirm", "")
                : new UserDTM
                {
                   Id = usr.Id,
                   FirstName = usr.FirstName,
                   LastName = usr.LastName,
                   AvatarPath = usr.AvatarPath,
                   Login = usr.Login,
                   Password = usr.Password,
                   Salt = usr.Salt,
                };
      }

      public async Task<UserDTM> CreatePassword(UserDTM item, string? passwordToHash) =>
         await Task.Run(() =>
         {
            byte[] saltbuf = new byte[16];

            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltbuf);

            StringBuilder sb = new(16);
            for (int i = 0; i < 16; i++)
               sb.Append(string.Format("{0:X2}", saltbuf[i]));

            string salt = sb.ToString();
            byte[] password = Encoding.Unicode.GetBytes(salt + passwordToHash);
            byte[] hashPassword = SHA256.HashData(password);

            StringBuilder hash = new(hashPassword.Length);
            for (int i = 0; i < hashPassword.Length; i++)
               hash.Append(string.Format("{0:X2}", hashPassword[i]));

            item.Password = hash.ToString();
            item.Salt = salt;

            return item;
         });

      public async Task<bool> IsPasswordCorrect(UserToConfirm user, string? passwordToCompare) =>
          await Task.Run(() =>
          {
             string? salt = user.Salt;
             byte[] password = Encoding.Unicode.GetBytes(salt + passwordToCompare);
             byte[] hashPassword = SHA256.HashData(password);

             StringBuilder hash = new(hashPassword.Length);
             for (int i = 0; i < hashPassword.Length; i++)
                hash.Append(string.Format("{0:X2}", hashPassword[i]));

             return user.Password == hash.ToString();
          });

   }
}
