using BusinessObject.Models;
using DataAccess;
using Repositories.IRepository;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDAO _dao;

        public TransactionRepository(TransactionDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(Transaction obj) => _dao.CreateWithdrawalRequest(obj);

        public Task DeleteAsync(int id) => _dao.DeleteWithdrawalRequest(id);

        public Task<Transaction> GetAsyncById(int id) => _dao.GetWithdrawalRequestById(id);

        public Task<List<Transaction>> GetsAsync() => _dao.GetWithdrawalRequests();

        public Task UpdateAsync(int id, Transaction obj) => _dao.UpdateWithdrawalRequest(id, obj);

        public Task<List<Transaction>> GetTransactionsByStatusAsync(string status, string transactionType) => _dao.GetTransactionsByStatusAsync(status, transactionType);

        public Task<List<Transaction>> GetDepositsByUserIdAsync(int userId) => _dao.GetDepositsByUserIdAsync(userId);

        public Task<bool> UpdateTransactionStatusAsync(int transactionId, string newStatus, DateTime processedDate) => _dao.UpdateTransactionStatusAsync(transactionId, newStatus, processedDate);

        public Task<List<Transaction>> GetDepositsByStatusAsync(string status) => _dao.GetDepositsByStatusAsync(status);

        public Task<List<Transaction>> GetWithdrawalsByStatusAsync(string status) => _dao.GetWithdrawalsByStatusAsync(status);
    }
}