using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<List<Transaction>> GetTransactionsByStatusAsync(string status, string transactionType);
        public Task<List<Transaction>> GetDepositsByUserIdAsync(int userId);
        public Task<bool> UpdateTransactionStatusAsync(int transactionId, string newStatus, DateTime processedDate);
        public Task<List<Transaction>> GetDepositsByStatusAsync(string status);
        public Task<List<Transaction>> GetWithdrawalsByStatusAsync(string status);
    }
}
