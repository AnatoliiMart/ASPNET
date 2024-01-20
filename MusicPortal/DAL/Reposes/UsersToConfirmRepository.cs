using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HearMe.DAL.Reposes
{
   public class UsersToConfirmRepository : IRepository<UserToConfirm>
   {
      private readonly MyDbContext _context;

      public UsersToConfirmRepository(MyDbContext context) => _context = context;
      public async Task Create(UserToConfirm item) => await _context.UsersToConfirm.AddAsync(item);

      public async Task Delete(int id)
      {
         UserToConfirm? usr = await _context.UsersToConfirm.FindAsync(id);
         if (usr != null)
            _context.UsersToConfirm.Remove(usr);
      }

      public async Task<UserToConfirm?> Get(int id) =>
         await _context.UsersToConfirm.FindAsync(id);

      // has been searched by LOGIN!!! 
      public async Task<UserToConfirm?> Get(string name) =>
         await _context.UsersToConfirm.Where(usr => usr.Login == name).FirstOrDefaultAsync();

      public async Task<IEnumerable<UserToConfirm>> GetAll() => await _context.UsersToConfirm.ToListAsync();

      public void Update(UserToConfirm item) => _context.UsersToConfirm.Entry(item).State = EntityState.Modified;
   }
}
