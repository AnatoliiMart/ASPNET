using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;
using MyMusicPortal.Models.ViewModels;

namespace MyMusicPortal.Reposes
{
    public interface IAccountRepository
    {
        Task<List<User>> GetAllUsers();

        IQueryable<User> GetUsersByLogin(string? login);

        Task<bool> IsPasswordCorrect(User user, string? passwordToCompare);

        Task<UserToConfirm> CreateAndHashPassword(UserToConfirm user, string? passwordToHash);

        Task<bool> IsLoginExists(string? login);

        Task AddUserOnConfirm(UserToConfirm user);
        Task AddConfirmedUser(User user);
    }
}
