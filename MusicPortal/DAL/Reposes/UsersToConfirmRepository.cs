using DAL.EF;
using HearMe.DAL.Entities;
using HearMe.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HearMe.DAL.Reposes
{
   public class UsersToConfirmRepository : IRepository<UsersToConfirm>
   {
      private readonly MyDbContext _context;

      public UsersToConfirmRepository(MyDbContext context) => _context = context;
      public async Task Create(UsersToConfirm item) => await _context.UsersToConfirm.AddAsync(item);

      public async Task Delete(int id)
      {
         UsersToConfirm? usr = await _context.UsersToConfirm.FindAsync(id);
         if (usr != null)
            _context.UsersToConfirm.Remove(usr);
      }

      public async Task<UsersToConfirm?> Get(int id) =>
         await _context.UsersToConfirm.FindAsync(id);

      // has been searched by LOGIN!!! 
      public async Task<UsersToConfirm?> Get(string name) =>
         await _context.UsersToConfirm.Where(usr => usr.Login == name).FirstOrDefaultAsync();

      public async Task<IEnumerable<UsersToConfirm>> GetAll() => await _context.UsersToConfirm.ToListAsync();

      public void Update(UsersToConfirm item) => _context.UsersToConfirm.Entry(item).State = EntityState.Modified;
   }
}
