using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Reposes;
using System.Security.Cryptography;
using System.Text;

namespace MyMusicPortal.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<UserToConfirm> UsersToConfirm { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                User user = new() {
                    Name = "Admin",
                    Surname = "Admin",
                    Login = "admin"
                };
                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));

                string salt = sb.ToString();
                byte[] password = Encoding.Unicode.GetBytes(salt + "admin123");
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
