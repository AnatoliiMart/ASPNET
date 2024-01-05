using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _context;

        public AccountRepository(MyDbContext context) => _context = context;

        public async Task<List<User>> GetAllUsers() => await _context.Users.ToListAsync();
    }
}
