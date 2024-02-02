using System.Security.Cryptography;
using System.Text;
using HearMe.BLL.DTM;
using HearMe.BLL.Infrasrtructure;
using HearMe.BLL.Interfaces;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;

namespace HearMe.BLL.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService(IUnitOfWork dataBase) => DataBase = dataBase;

        public IUnitOfWork DataBase { get; private set; }

        public async Task<User> CreatePassword(User item, string? passwordToHash) =>
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

        public async Task<UserToConfirm> CreatePassword(UserToConfirm item, string? passwordToHash) =>
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

        public async Task<bool> IsPasswordCorrect(UserDTM usr, string? passwordToCompare) =>
           await Task.Run(async () =>
           {
               User? user = await DataBase.Users.Get(usr.Id);
               if (user == null)
                   return false;

               string? salt = user.Salt;
               byte[] password = Encoding.Unicode.GetBytes(salt + passwordToCompare);
               byte[] hashPassword = SHA256.HashData(password);

               StringBuilder hash = new(hashPassword.Length);
               for (int i = 0; i < hashPassword.Length; i++)
                   hash.Append(string.Format("{0:X2}", hashPassword[i]));

               return user.Password == hash.ToString();
           });

        public async Task ChangePassword(int id, string? changedPassword)
        {
            User? user = await DataBase.Users.Get(id) ?? throw new ValidationException("", "User not found");
            await CreatePassword(user, changedPassword);
        }
    }
}
