using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;

namespace Services
{
    public class VoucherAdminService
    {
        private readonly IVoucherAdminRepository _voucherAdminRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VoucherAdminService(
            IVoucherAdminRepository voucherAdminRepository,
            IUnitOfWork unitOfWork)
        {
            _voucherAdminRepository = voucherAdminRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VoucherAdminDTO>> GetAllVoucherAdminsAsync()
        {
            try
            {
                List<VoucherAdmin> vouchers = await _voucherAdminRepository.GetsAsync();
                return vouchers.Select(v => new VoucherAdminDTO
                {
                    VoucherId = v.VoucherId,
                    TypeOfCombo = v.TypeOfCombo,
                    TypeOfDiscount = v.TypeOfDiscount,
                    Percent = v.Percent,
                    Amount = v.Amount,
                    StartDate = v.StartDate,
                    EndDate = v.EndDate
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<VoucherAdminDTO>> GetUpcomingVouchersAsync()
        {
            try
            {
                List<VoucherAdmin> vouchers = await _voucherAdminRepository.GetUpcomingVouchersAsync();
                return vouchers.Select(v => new VoucherAdminDTO
                {
                    VoucherId = v.VoucherId,
                    TypeOfCombo = v.TypeOfCombo,
                    TypeOfDiscount = v.TypeOfDiscount,
                    Percent = v.Percent,
                    Amount = v.Amount,
                    StartDate = v.StartDate,
                    EndDate = v.EndDate
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<VoucherAdminDTO?> GetVoucherAdminByIdAsync(int id)
        {
            try
            {
                VoucherAdmin voucher = await _voucherAdminRepository.GetAsyncById(id);
                if (voucher == null) return null;

                return new VoucherAdminDTO
                {
                    VoucherId = voucher.VoucherId,
                    TypeOfCombo = voucher.TypeOfCombo,
                    TypeOfDiscount = voucher.TypeOfDiscount,
                    Percent = voucher.Percent,
                    Amount = voucher.Amount,
                    StartDate = voucher.StartDate,
                    EndDate = voucher.EndDate
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateVoucherAdminAsync(VoucherAdminDTO voucherDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                VoucherAdmin voucher = new()
                {
                    TypeOfCombo = voucherDto.TypeOfCombo,
                    TypeOfDiscount = voucherDto.TypeOfDiscount,
                    Percent = voucherDto.Percent,
                    Amount = voucherDto.Amount,
                    StartDate = voucherDto.StartDate,
                    EndDate = voucherDto.EndDate
                };

                await _voucherAdminRepository.CreateAsync(voucher);

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
        public async Task<bool> UpdateVoucherAdminAsync(int id, VoucherAdminDTO voucherDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                VoucherAdmin existingVoucher = await _voucherAdminRepository.GetAsyncById(id);
                if (existingVoucher == null) return false;

                existingVoucher.TypeOfCombo = voucherDto.TypeOfCombo;
                existingVoucher.TypeOfDiscount = voucherDto.TypeOfDiscount;
                existingVoucher.Percent = voucherDto.Percent;
                existingVoucher.Amount = voucherDto.Amount;
                existingVoucher.StartDate = voucherDto.StartDate;
                existingVoucher.EndDate = voucherDto.EndDate;

                await _voucherAdminRepository.UpdateAsync(id, existingVoucher);

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
        public async Task<bool> DeleteVoucherAdminAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _voucherAdminRepository.DeleteAsync(id);

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
