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
   public class UserService : IModelService<UserDTM>, IPasswordCreation<User>
   {
      public IUnitOfWork DataBase { get; protected set; }

      public UserService(IUnitOfWork unit) => DataBase = unit;

      public async Task CreateItem(UserDTM user)
      {
         var usr = new User
         {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarPath = user.AvatarPath,
            Login = user.Login,
            Password = user.Password,
            Salt = user.Salt,
         };
         await DataBase.Users.Create(usr);
         await DataBase.Save();
      }

      public async Task DeleteItem(int id)
      {
         await DataBase.Users.Delete(id);
         await DataBase.Save();
      }


      public async Task<IEnumerable<UserDTM>> GetItemsList()
      {
         var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTM>()).CreateMapper();
         return mapper.Map<IEnumerable<User>, IEnumerable<UserDTM>>(await DataBase.Users.GetAll());
      }

      public async Task<UserDTM> GetItem(int id)
      {
         User? usr = await DataBase.Users.Get(id);
         return usr == null
                ? throw new ValidationException("This User does not found in our base of Users", "")
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

      public async Task UpdateItem(UserDTM user)
      {
         var usr = new User
         {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            AvatarPath = user.AvatarPath,
            Password = user.Password,
            Salt = user.Salt
         };
         DataBase.Users.Update(usr);
         await DataBase.Save();
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

      public async Task<bool> IsPasswordCorrect(User user, string? passwordToCompare) =>
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
