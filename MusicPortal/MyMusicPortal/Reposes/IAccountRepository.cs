using MyMusicPortal.Models;

namespace MyMusicPortal.Reposes
{
    public interface IAccountRepository
    {
        Task<List<User>> GetAllUsers();

        Task<List<UserToConfirm>> GetAllUsersToConfirm();

        Task<UserToConfirm?> GetUserToConfirmById(int id);

        IQueryable<User> GetUsersByLogin(string? login);

        Task<bool> IsPasswordCorrect(User user, string? passwordToCompare);

        Task<UserToConfirm> CreateAndHashPassword(UserToConfirm user, string? passwordToHash);

        Task<bool> IsLoginExists(string? login);

        Task AddUserOnConfirm(UserToConfirm user);

        Task RemoveUserFromConfirmationList(UserToConfirm usr);

        Task AddConfirmedUser(User user);
    }
}
