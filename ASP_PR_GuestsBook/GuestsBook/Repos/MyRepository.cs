using GuestsBook.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace GuestsBook.Repos
{
    public class MyRepository : IMyRepository
    {
        private readonly MyDbContext _context;

        public MyRepository(MyDbContext context) => _context = context;
       




        public async Task<List<Message>> GetAllMessages()
        {
            var myDbContext = _context.Messages.Include(x => x.User);
            return await myDbContext.ToListAsync();
        }

        public async Task<List<Message>> GetSelfMessages(User concreteUser)
        {
            var myDbContext = _context.Messages.Where(m => m.User.Id == concreteUser.Id);
            return await myDbContext.ToListAsync();
        }

        public async Task<Message?> GetMessageDetails(int? id) =>
            await _context.Messages
                  .Include(m => m.User)
                  .FirstOrDefaultAsync(m => m.Id == id);

        public IQueryable<User> GetUserByLogin(string? login) => _context.Users.Where(x => x.Login == login);

        public async Task<Message?> GetMessage(int? id) => await _context.Messages.FindAsync(id);

        public async Task SaveChanges() => await _context.SaveChangesAsync();

        public void UpdateMessage(Message msg) => _context.Update(msg); 
        public async Task CreateMessage(Message msg) => await _context.AddAsync(msg);        
        public async Task DeleteMessage(int id)
        {
            var message = await GetMessage(id);

            if (message != null)
                _context.Messages.Remove(message);
        }

       public async Task<bool> MessageExists(int id) => await _context.Messages.AnyAsync(e => e.Id == id);
    }
}
