using HearMe.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Song> Songs { get; set; }

        public MyDbContext(DbContextOptions options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                Users.Add(new User() { FirstName = "Admin", LastName = "Admin"})
            }
        }
    }
}
