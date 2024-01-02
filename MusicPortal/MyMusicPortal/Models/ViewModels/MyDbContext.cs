using Microsoft.EntityFrameworkCore;

namespace MyMusicPortal.Models.ViewModels
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) => Database.EnsureCreated();

        public DbSet<User> Users { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Genre> Genres { get; set; }
    }
}
