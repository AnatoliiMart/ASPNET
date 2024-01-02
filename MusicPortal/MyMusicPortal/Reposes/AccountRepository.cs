using MyMusicPortal.Models.ViewModels;

namespace MyMusicPortal.Reposes
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _context;

        public AccountRepository(MyDbContext context) => _context = context;
    }
}
