using BusinessObject.Models;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WeddingWonderDbContext _context;
        public UnitOfWork(WeddingWonderDbContext context)
        {
            _context = context;
        }
        public Task BeginTransactionAsync() => _context.Database.BeginTransactionAsync();

        public Task CommitAsync() => _context.SaveChangesAsync();

        public Task CommitTransactionAsync() => _context.Database.CommitTransactionAsync();

        public Task RollbackTransactionAsync() => _context.Database.RollbackTransactionAsync();
    }
}
