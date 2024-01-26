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

        public MyDbContext(DbContextOptions options) : base(options) => Database.EnsureCreated();
    }
}
