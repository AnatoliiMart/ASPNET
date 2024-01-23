using System.Security.Cryptography;
using System.Text;
using HearMe.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<UserToConfirm> UsersToConfirm { get; set; }

        public MyDbContext(DbContextOptions options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                User user = new()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Login = "admin",
                    IsAdmin = true,
                };
                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));

                string salt = sb.ToString();
                byte[] password = Encoding.Unicode.GetBytes(salt + "admin");
                byte[] hashPassword = SHA256.HashData(password);

                StringBuilder hash = new(hashPassword.Length);
                for (int i = 0; i < hashPassword.Length; i++)
                    hash.Append(string.Format("{0:X2}", hashPassword[i]));

                user.Password = hash.ToString();
                user.Salt = salt;

                Users?.Add(user);
                SaveChanges();
            }
        }
    }
}
