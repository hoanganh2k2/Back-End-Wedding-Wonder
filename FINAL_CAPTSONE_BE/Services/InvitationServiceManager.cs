using BusinessObject.Models;
using Repository.IRepositories;
using WeddingWonderAPI.Models.DTO;

namespace Services
{
    public class InvitationServiceManager
    {
        private readonly IInvitationPackageRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InvitationServiceManager(
            IInvitationPackageRepository invitationRepository,
            IUnitOfWork unitOfWork)
        {
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<InvitationPackageDTO>> GetAllInvitationsAsync()
        {
            try
            {
                List<InvitationPackage> packages = await _invitationRepository.GetsAsync();
                return packages.Select(p => new InvitationPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0m,
                    PackageDescribe = p.PackageDescribe,
                    Envelope = p.Envelope,
                    Component = p.Component,
                    Size = p.Size,
                    Status = p.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<InvitationPackageDTO?> GetInvitationByIdAsync(int id)
        {
            try
            {
                InvitationPackage package = await _invitationRepository.GetAsyncById(id);
                if (package == null) return null;

                return new InvitationPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0m,
                    PackageDescribe = package.PackageDescribe,
                    Envelope = package.Envelope,
                    Component = package.Component,
                    Size = package.Size,
                    Status = package.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<decimal> GetPriceByPackageId(int id, int number)
        {
            try
            {
                InvitationPackage invitation = await _invitationRepository.GetAsyncById(id);
                if (invitation == null) return 0;

                decimal price = 0;
                decimal packagePrice = invitation.PackagePrice ?? 0;

                if (number <= 100) price = packagePrice * number;
                else if (number <= 200) price = packagePrice * number * 90 / 100;
                else if (number <= 300) price = packagePrice * number * 80 / 100;
                else price = packagePrice * number * 70 / 100;

                return price;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<InvitationPackageDTO>> GetCardPackagesByServiceIdAsync(int serviceid)
        {
            try
            {
                List<InvitationPackage> packages = await _invitationRepository.GetInvitationPackagesByServiceId(serviceid);

                if (packages == null || !packages.Any()) return new List<InvitationPackageDTO>();

                List<InvitationPackageDTO> packageDTOs = packages.Select(package => new InvitationPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageDescribe = package.PackageDescribe,
                    Envelope = package.Envelope,
                    Component = package.Component,
                    Size = package.Size,
                    Status = package.Status
                }).ToList();

                return packageDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateInvitationAsync(InvitationPackageDTO invitationDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                InvitationPackage package = new()
                {
                    PackageName = invitationDto.PackageName,
                    PackagePrice = invitationDto.PackagePrice,
                    ServiceId = invitationDto.ServiceId,
                    PackageDescribe = invitationDto.PackageDescribe,
                    Envelope = invitationDto.Envelope,
                    Component = invitationDto.Component,
                    Size = invitationDto.Size,
                    Status = invitationDto.Status
                };

                await _invitationRepository.CreateAsync(package);
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

        public async Task<bool> UpdateInvitationAsync(int id, InvitationPackageDTO packageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                InvitationPackage existingPackage = await _invitationRepository.GetAsyncById(id);
                if (existingPackage == null) return false;

                existingPackage.PackageName = packageDto.PackageName;
                existingPackage.PackagePrice = packageDto.PackagePrice;
                existingPackage.PackageDescribe = packageDto.PackageDescribe;
                existingPackage.Envelope = packageDto.Envelope;
                existingPackage.Component = packageDto.Component;
                existingPackage.Size = packageDto.Size;
                existingPackage.Status = packageDto.Status;

                await _invitationRepository.UpdateAsync(id, existingPackage);
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

        public async Task<bool> DeleteInvitationAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _invitationRepository.DeleteAsync(id);
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