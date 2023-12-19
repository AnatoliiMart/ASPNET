using Microsoft.EntityFrameworkCore;

namespace GuestsBook.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) => Database.EnsureCreated();
    }
}
