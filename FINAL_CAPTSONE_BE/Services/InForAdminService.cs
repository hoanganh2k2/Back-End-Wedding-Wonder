using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;

namespace Services
{
    public class InForAdminService
    {
        private readonly IInForAdminRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public InForAdminService(IInForAdminRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<InForAdminDTO>> GetAllInForAdminsAsync()
        {
            List<InforAdmin> admins = await _repository.GetsAsync();
            return admins.ConvertAll(admin => new InForAdminDTO
            {
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber,
                Address = admin.Address,
                Description = admin.Description
            });
        }
        public async Task<InForAdminDTO?> GetInForAdminByEmailAsync(string email)
        {
            InforAdmin? admin = await _repository.GetByEmailAsync(email);
            return admin == null ? null : new InForAdminDTO
            {
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber,
                Address = admin.Address,
                Description = admin.Description
            };
        }
        public async Task<bool> CreateInForAdminAsync(InForAdminDTO dto)
        {
            InforAdmin admin = new()
            {
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Description = dto.Description
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.CreateAsync(admin);
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
        public async Task<bool> UpdateInForAdminAsync(string email, InForAdminDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                InforAdmin? existingAdmin = await _repository.GetByEmailAsync(email);
                if (existingAdmin == null) return false;

                existingAdmin.PhoneNumber = dto.PhoneNumber;
                existingAdmin.Address = dto.Address;
                existingAdmin.Description = dto.Description;

                await _repository.UpdateByEmailAsync(email, existingAdmin);
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
        public async Task<bool> DeleteInForAdminAsync(string email)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.DeleteByEmailAsync(email);
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
    }
}
