namespace Repositories.IRepository
{
    public interface IRepository<T>
    {
        Task<bool> CreateAsync(T obj);
        Task<T> GetAsyncById(int id);
        Task<List<T>> GetsAsync();
        Task UpdateAsync(int id, T obj);
        Task DeleteAsync(int id);
    }
}
