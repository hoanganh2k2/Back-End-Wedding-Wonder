using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    using BusinessObject.DTOs;
    using BusinessObject.Models;
    using DataAccess;
    using Repositories.IRepository;
    using Repository.IRepositories;
    using Repository.Repositories;

    namespace Services
    {
        public class TransactionService
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;

            public TransactionService(
                ITransactionRepository transactionRepository,
                IUnitOfWork unitOfWork, IUserRepository userRepository)
            {
                _transactionRepository = transactionRepository;
                _unitOfWork = unitOfWork;
                _userRepository = userRepository;
            }

            public async Task<List<TransactionDTO>> GetAllWithdrawalRequestsAsync()
            {
                try
                {
                    List<Transaction> requests = await _transactionRepository.GetsAsync();
                    return requests.Select(r => new TransactionDTO
                    {
                        TransactionId = r.TransactionId,
                        UserId = r.UserId,
                        Amount = r.Amount,
                        Reason = r.Reason,
                        RequestDate = r.RequestDate,
                        Status = r.Status,
                        ProcessedDate = r.ProcessedDate
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            public async Task<List<TransactionDTO>> GetTransactionsByStatusAsync(string status, string transactionType)
            {
                var transactions = await _transactionRepository.GetTransactionsByStatusAsync(status, transactionType);
                return transactions.Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    UserId = t.UserId,
                    Amount = t.Amount,
                    Reason = t.Reason,
                    RequestDate = t.RequestDate,
                    Status = t.Status,
                    ProcessedDate = t.ProcessedDate,
                    TransactionType = t.TransactionType,
                    CardNumber = t.CardNumber,
                    CardHolderName = t.CardHolderName,
                    BankName = t.BankName
                }).ToList();
            }

            public async Task<List<TransactionDTO>> GetDepositsByStatusAsync(string status)
            {
                var deposits = await _transactionRepository.GetDepositsByStatusAsync(status);
                return deposits.Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    UserId = t.UserId,
                    Amount = t.Amount,
                    Reason = t.Reason,
                    RequestDate = t.RequestDate,
                    Status = t.Status,
                    ProcessedDate = t.ProcessedDate,
                    TransactionType = t.TransactionType,
                    CardNumber = t.CardNumber,
                    CardHolderName = t.CardHolderName,
                    BankName = t.BankName
                }).ToList();
            }

            public async Task<List<TransactionDTO>> GetWithdrawalsByStatusAsync(string status)
            {
                var withdrawals = await _transactionRepository.GetWithdrawalsByStatusAsync(status);
                return withdrawals.Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    UserId = t.UserId,
                    Amount = t.Amount,
                    Reason = t.Reason,
                    RequestDate = t.RequestDate,
                    Status = t.Status,
                    ProcessedDate = t.ProcessedDate,
                    TransactionType = t.TransactionType,
                    CardNumber = t.CardNumber,
                    CardHolderName = t.CardHolderName,
                    BankName = t.BankName
                }).ToList();
            }

            public async Task<List<TransactionDTO>> GetDepositsByUserIdAsync(int userId)
            {
                var transactions = await _transactionRepository.GetDepositsByUserIdAsync(userId);
                return transactions.Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    UserId = t.UserId,
                    Amount = t.Amount,
                    Reason = t.Reason,
                    RequestDate = t.RequestDate,
                    Status = t.Status,
                    ProcessedDate = t.ProcessedDate,
                    TransactionType = t.TransactionType,
                    CardNumber = t.CardNumber,
                    CardHolderName = t.CardHolderName,
                    BankName = t.BankName
                }).ToList();
            }

            public async Task<TransactionDTO?> GetWithdrawalRequestByIdAsync(int id)
            {
                try
                {
                    Transaction request = await _transactionRepository.GetAsyncById(id);
                    if (request == null) return null;

                    return new TransactionDTO
                    {
                        TransactionId = request.TransactionId,
                        UserId = request.UserId,
                        Amount = request.Amount,
                        Reason = request.Reason,
                        RequestDate = request.RequestDate,
                        Status = request.Status,
                        ProcessedDate = request.ProcessedDate,
                        TransactionType= request.TransactionType,
                        CardNumber = request.CardNumber,
                        CardHolderName = request.CardHolderName,
                        BankName = request.BankName
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            public async Task<bool> CreateTransactionRequestAsync(
                int userId,
                double amount,
                string transactionType,
                string status,
                string reason = null,
                DateTime? requestDate = null,
                DateTime? processedDate = null,
                string cardHolderName = null,
                string cardNumber = null,
                string bankName = null)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    Transaction transaction = new Transaction
                    {
                        UserId = userId,
                        Amount = amount,
                        TransactionType = transactionType,
                        Status = status,
                        Reason = reason ?? "Default reason",
                        RequestDate = requestDate ?? DateTime.Now,
                        ProcessedDate = processedDate ?? DateTime.Now,
                        CardHolderName = cardHolderName,
                        CardNumber = cardNumber,
                        BankName = bankName
                    };

                    await _transactionRepository.CreateAsync(transaction);

                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception(ex.Message, ex);
                }
            } 

        public async Task<bool> UpdateWithdrawalRequestAsync(int id, TransactionDTO requestDto)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    Transaction existingRequest = await _transactionRepository.GetAsyncById(id);
                    if (existingRequest == null) return false;

                    existingRequest.UserId = requestDto.UserId;
                    existingRequest.Amount = requestDto.Amount;
                    existingRequest.Reason = requestDto.Reason;
                    existingRequest.RequestDate = requestDto.RequestDate;
                    existingRequest.Status = requestDto.Status;
                    existingRequest.ProcessedDate = requestDto.ProcessedDate;

                    await _transactionRepository.UpdateAsync(id, existingRequest);

                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception(ex.Message, ex);
                }
            }

            public async Task<bool> DeleteWithdrawalRequestAsync(int id)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    await _transactionRepository.DeleteAsync(id);

                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception(ex.Message, ex);
                }
            }
            public async Task<bool> AcpWithdrawRequestAsync(int id)
            {
                return await UpdateRequestStatusAsync(id, "2", adjustBalance: true);
            }

            public async Task<bool> RejectWithdrawRequestAsync(int id)
            {
                return await UpdateRequestStatusAsync(id, "3");
            }

            private async Task<bool> UpdateRequestStatusAsync(int id, string newStatus, bool adjustBalance = false)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    var request = await _transactionRepository.GetAsyncById(id);
                    if (request == null) return false;

                    request.Status = newStatus;
                    request.ProcessedDate = DateTime.Now;

                    if (adjustBalance)
                    {
                        var user = await _userRepository.GetAsyncById((int)request.UserId);
                        if (user == null) return false;

                        if (user.Balance < request.Amount)
                        {
                            throw new InvalidOperationException("Tiền ít mà đòi rút hết!!");
                        }

                        user.Balance = (user.Balance ?? 0) - request.Amount; 
                        await _userRepository.UpdateAsync(user.UserId, user);
                    }

                    await _transactionRepository.UpdateAsync(request.TransactionId, request);
                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception($"{ex.Message}", ex);
                }
            }
        }
    }
}
