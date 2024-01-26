namespace HearMe.BLL.Interfaces
{
    public interface IModelService<T> where T : class
    {
        Task CreateItem(T item);

        Task UpdateItem(T? item);

        Task DeleteItem(int id);

        Task<T> GetItem(int id);

        Task<IEnumerable<T>> GetItemsList();
    }
}
