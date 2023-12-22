using GuestsBook.Models;

namespace GuestsBook.Repos
{
    public interface IAccountsRepository
    {
        Task<List<User>> GetAllUsers();

        IQueryable<User> GetUsersByLogin(LoginMDL user);

        Task<User> CreateAndHashPassword(User user, RegistMDL model);

        Task<bool> IsPasswordCorrect(User user, LoginMDL model);

        Task<bool> IsLoginExists(string? login);

        Task AddUserToDb(User user);
    }
}
