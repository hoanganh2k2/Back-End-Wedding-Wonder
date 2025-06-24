using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionDAO
    {
        private readonly WeddingWonderDbContext context;

        public TransactionDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Transaction>> GetWithdrawalRequests()
        {
            try
            {
                return await context.Transactions.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Transaction> GetWithdrawalRequestById(int transactionId)
        {
            try
            {
                return await context.Transactions.FindAsync(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        } 

        public async Task<bool> CreateWithdrawalRequest(Transaction transaction)
        {
            try
            {
                await context.Transactions.AddAsync(transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateWithdrawalRequest(int transactionId, Transaction withdrawalRequest)
        {
            try
            {
                context.Transactions.Update(withdrawalRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteWithdrawalRequest(int transactionId)
        {
            try
            {
                Transaction? requestToDelete = await context.Transactions.FindAsync(transactionId);
                if (requestToDelete != null)
                {
                    context.Transactions.Remove(requestToDelete);
                }
                else
                {
                    throw new Exception("WithdrawalRequest not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Transaction>> GetTransactionsByStatusAsync(string status, string transactionType)
        {
            try
            {
                return await context.Transactions
                    .Where(t => t.Status == status && t.TransactionType == transactionType)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching transactions by status: " + ex.Message, ex);
            }
        }

        public async Task<List<Transaction>> GetDepositsByStatusAsync(string status)
        {
            return await GetTransactionsByStatusAsync(status, "Deposit");
        }

        public async Task<List<Transaction>> GetWithdrawalsByStatusAsync(string status)
        {
            return await GetTransactionsByStatusAsync(status, "Withdraw");
        }

        public async Task<List<Transaction>> GetDepositsByUserIdAsync(int userId)
        {
            try
            {
                return await context.Transactions
                    .Where(t => t.UserId == userId && t.TransactionType == "Deposit")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching deposits by user ID: " + ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTransactionStatusAsync(int transactionId, string newStatus, DateTime processedDate)
        {
            try
            {
                var transaction = await context.Transactions.FindAsync(transactionId);
                if (transaction == null) return false;

                transaction.Status = newStatus;
                transaction.ProcessedDate = processedDate;
                context.Transactions.Update(transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating transaction status: " + ex.Message, ex);
            }
        }
    }
}