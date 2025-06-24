namespace Repository.IRepositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
