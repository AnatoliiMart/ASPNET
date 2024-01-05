using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes
{
    public interface IAccountRepository
    {
        Task<List<User>> GetAllUsers();
    }
}
