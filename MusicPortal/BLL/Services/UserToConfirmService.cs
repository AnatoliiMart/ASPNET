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
   public class UserToConfirmService : IUserToConfirmService, IPasswordCreation
   {
      IUnitOfWork DataBase { get; set; }

      public UserToConfirmService(IUnitOfWork unit) => DataBase = unit;

      public async Task CreateUserToConfirm(UserDTM user)
      {
         var usr = new UsersToConfirm
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

      public async Task DeleteUserToConfirm(int id) =>
         await DataBase.UsersToConfirm.Delete(id);

      public async Task<IEnumerable<UserDTM>> GetAllUsersToConfirm()
      {
         var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UsersToConfirm, UserDTM>()).CreateMapper();
         return mapper.Map<IEnumerable<UsersToConfirm>, IEnumerable<UserDTM>>(await DataBase.UsersToConfirm.GetAll());
      }

      public async Task<UserDTM> GetUserToConfirm(int id)
      {
         UsersToConfirm? usr = await DataBase.UsersToConfirm.Get(id);
         if (usr == null)
            throw new ValidationException("This team does not exists", "");
         return new UserDTM
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
   }
}
