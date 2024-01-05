using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyMusicPortal.Reposes
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _context;

        public AccountRepository(MyDbContext context) => _context = context;

        public async Task<List<User>> GetAllUsers() => await _context.Users.ToListAsync();

        public async Task<UserToConfirm?> GetUserToConfirmById(int id) =>
            await _context.UsersToConfirm.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<UserToConfirm>> GetAllUsersToConfirm() => await _context.UsersToConfirm.ToListAsync();

        public IQueryable<User> GetUsersByLogin(string? login) => _context.Users.Where(x => x.Login == login);

        public async Task<UserToConfirm> CreateAndHashPassword(UserToConfirm user, string? passwordToHash) =>
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

                user.Password = hash.ToString();
                user.Salt = salt;

                return user;
            });

        public async Task<bool> IsPasswordCorrect(User user, string? passwordToCompare) =>
            await Task.Run(() =>
            {
                string salt = user.Salt;
                byte[] password = Encoding.Unicode.GetBytes(salt + passwordToCompare);
                byte[] hashPassword = SHA256.HashData(password);

                StringBuilder hash = new(hashPassword.Length);
                for (int i = 0; i < hashPassword.Length; i++)
                    hash.Append(string.Format("{0:X2}", hashPassword[i]));

                return user.Password != hash.ToString();
            });

        public async Task<bool> IsLoginExists(string? login) =>
            await _context.Users.AnyAsync(x => x.Login == login) || await _context.UsersToConfirm.AnyAsync(x=> x.Login == login);

        public async Task AddUserOnConfirm(UserToConfirm user)
        {
            await _context.UsersToConfirm.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromConfirmationList(UserToConfirm usr)
        {
            _context.UsersToConfirm.Remove(usr);
            await _context.SaveChangesAsync();
        }

        public async Task AddConfirmedUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
