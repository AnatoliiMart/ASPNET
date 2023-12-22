using GuestsBook.Models;

namespace GuestsBook.Repos
{
    public interface IMessagesRepository
    {
        Task<List<Message>> GetAllMessages();

        Task<List<Message>> GetSelfMessages(User userSpecification);

        Task<Message?> GetMessageDetails(int? id);

        IQueryable<User> GetUserByLogin(string? login);

        Task<Message?> GetMessage(int? id);

        Task CreateMessage(Message msg);

        void UpdateMessage(Message msg);

        Task DeleteMessage(int id);

        Task<bool> MessageExists(int id);

        Task SaveChanges();
    }
}
