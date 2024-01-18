using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HearMe.DAL.Reposes
{
   public class UsersRepository : IRepository<User>
   {
      private readonly MyDbContext _context;

      public UsersRepository(MyDbContext context) => _context = context;

      public async Task Create(User item) =>
         await _context.Users.AddAsync(item);

      public async Task Delete(int id)
      {
         User? usr = await _context.Users.FindAsync(id);
         if (usr != null)
            _context.Users.Remove(usr);
      }

      public async Task<User?> Get(int id) => await _context.Users.FindAsync(id);

      // has been searched by LOGIN!!! 
      public async Task<User?> Get(string name) =>
         await _context.Users.Where(usr => usr.Login == name).FirstOrDefaultAsync();

      public async Task<IEnumerable<User>> GetAll() => await _context.Users.ToListAsync();

      public void Update(User item) => _context.Users.Entry(item).State = EntityState.Modified;
   }
}
