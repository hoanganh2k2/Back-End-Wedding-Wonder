using BusinessObject.DTOs;
using BusinessObject.Models;
using MimeKit;
using Repository.IRepositories;
using Services.Email;
using Message = Services.Email.Message;

namespace Services
{
    public class ContractService
    {
        private readonly IContractRepository _contractRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IEmailService _emailService;

        public ContractService(IContractRepository contractRepository, IEmailService emailService, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateContractAsync(ContractDTOCreate contractDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var userExists = await _userRepository.GetAsyncById(contractDto.UserId);
                if (userExists == null)
                {
                    throw new Exception("User does not exist.");
                }

                var contract = new Contract
                {
                    Title = contractDto.Title,
                    UserId = contractDto.UserId,
                    PdfFilePath = contractDto.PdfFilePath,
                    CreatedDate = DateTime.Now,
                    Otp = GenerateOtp()
                };

                await _contractRepository.CreateAsync(contract);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error creating contract: {ex.Message}. Inner Exception: {ex.InnerException?.Message}", ex);
            }
        }

        public async Task<List<Contract>> GetAllContractsAsync()
        {
            return await _contractRepository.GetsAsync();
        }

        public async Task<Contract> GetContractByIdAsync(int id)
        {
            return await _contractRepository.GetAsyncById(id);
        }

        public async Task UpdateContractAsync(int id, ContractDTOCreate contractDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingContract = await _contractRepository.GetAsyncById(id);
                if (existingContract == null)
                {
                    throw new Exception("Contract not found.");
                }

                existingContract.Title = contractDto.Title;
                existingContract.UserId = contractDto.UserId;

                await _contractRepository.UpdateAsync(id, existingContract);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error updating contract: {ex.Message}", ex);
            }
        }

        public async Task DeleteContractAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingContract = await _contractRepository.GetAsyncById(id);
                if (existingContract == null)
                {
                    throw new Exception("Contract not found.");
                }

                await _contractRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error deleting contract: {ex.Message}", ex);
            }
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> ConfirmContractAsync(int id, ContractConfirmDTO contractDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingContract = await _contractRepository.GetAsyncById(id);

                if (existingContract == null)
                {
                    throw new Exception("Contract not found.");
                }
                if (existingContract.Otp != contractDto.Otp)
                {
                    throw new Exception("Invalid OTP.");
                }

                existingContract.FrontIdCardPath = contractDto.FrontIdCardPath;
                existingContract.BackIdCardPath = contractDto.BackIdCardPath;
                existingContract.SignedDate = DateTime.Now;

                await _contractRepository.UpdateAsync(id, existingContract);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error confirming contract: {ex.Message}", ex);
            }
        }

        public async Task SendOtpEmailAsync(string userEmail, string otp, string userName)
        {
            string subject = "Your OTP Code";
            string content = $"Dear {userName},<br><br>" +
                             $"Your OTP code is: <strong>{otp}</strong><br><br>" +
                             "If you have any questions, please feel free to contact our support team.<br><br>" +
                             "Best regards,<br>" +
                             "Wedding Wonder Team";

            Message message = new(
                               new List<MailboxAddress> { new(userName, userEmail) },
                               subject,
                               content
                           );

            // Gửi email
            await _emailService.SendEmail(message);
        }

        public async Task SendOtpForContractConfirmationAsync(int id, string userEmail, string userName)
        {
            var existingContract = await _contractRepository.GetAsyncById(id);

            if (existingContract == null)
            {
                throw new Exception("Contract not found.");
            }

            // Gửi OTP qua email
            await SendOtpEmailAsync(userEmail, existingContract.Otp, userName);
        }
    }
}