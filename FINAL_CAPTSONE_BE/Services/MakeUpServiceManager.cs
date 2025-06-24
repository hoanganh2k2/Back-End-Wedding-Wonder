using BusinessObject.Models;
using Repository.IRepositories;
using WeddingWonderAPI.Models.DTOs;

namespace Services
{
    public class MakeUpServiceManager
    {
        private readonly IMakeUpPackageRepository _makeUpPackageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MakeUpServiceManager(
            IMakeUpPackageRepository makeUpPackageRepository,
            IUnitOfWork unitOfWork)
        {
            _makeUpPackageRepository = makeUpPackageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MakeUpPackageDTO>> GetAllMakeUpPackagesAsync()
        {
            try
            {
                List<MakeUpPackage> packages = await _makeUpPackageRepository.GetsAsync();
                return packages.Select(p => new MakeUpPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0,
                    PackageContent = p.PackageContent,
                    EventType = p.EventType,
                    Status = p.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<MakeUpPackageDTO?> GetMakeUpPackagesByIdAsync(int id)
        {
            try
            {
                MakeUpPackage package = await _makeUpPackageRepository.GetAsyncById(id);
                if (package == null) return null;

                return new MakeUpPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageContent = package.PackageContent,
                    EventType = package.EventType,
                    Status = package.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<decimal?> GetPriceByPackageId(int id)
        {
            try
            {
                MakeUpPackage package = await _makeUpPackageRepository.GetAsyncById(id);
                if (package == null) return 0;

                return package.PackagePrice;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<MakeUpPackageDTO>> GetMakeUpPackagesByServiceIdAsync(int serviceid)
        {
            try
            {
                List<MakeUpPackage> packages = await _makeUpPackageRepository.GetAsyncByServiceId(serviceid);

                if (packages == null || !packages.Any()) return new List<MakeUpPackageDTO>();

                List<MakeUpPackageDTO> packageDTOs = packages.Select(package => new MakeUpPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageContent = package.PackageContent,
                    EventType = package.EventType,
                    Status = package.Status
                }).ToList();

                return packageDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting make-up packages by service id: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateMakeUpPackagesAsync(MakeUpPackageDTO makeUpPackageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                MakeUpPackage package = new()
                {
                    PackageName = makeUpPackageDto.PackageName,
                    PackagePrice = makeUpPackageDto.PackagePrice,
                    ServiceId = makeUpPackageDto.ServiceId,
                    PackageContent = makeUpPackageDto.PackageContent,
                    EventType = makeUpPackageDto.EventType,
                    Status = makeUpPackageDto.Status
                };

                await _makeUpPackageRepository.CreateAsync(package);
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

        public async Task<bool> UpdateMakeUpPackagesAsync(int id, MakeUpPackageDTO packageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                MakeUpPackage existingPackage = await _makeUpPackageRepository.GetAsyncById(id);
                if (existingPackage == null) return false;

                existingPackage.PackageName = packageDto.PackageName;
                existingPackage.PackagePrice = packageDto.PackagePrice;
                existingPackage.PackageContent = packageDto.PackageContent;
                existingPackage.EventType = packageDto.EventType;
                existingPackage.Status = packageDto.Status;

                await _makeUpPackageRepository.UpdateAsync(id, existingPackage);
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

        public async Task<bool> DeleteMakeUpPackagesAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _makeUpPackageRepository.DeleteAsync(id);
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

        public async Task<List<MakeUpPackageDTO>> GetMakeUpPackagesByEventTypeAsync(int eventType)
        {
            try
            {
                List<MakeUpPackage> packages = await _makeUpPackageRepository.GetAsyncByEventType(eventType);

                return packages.Select(p => new MakeUpPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0,
                    PackageContent = p.PackageContent,
                    EventType = p.EventType,
                    Status = p.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting make-up packages by event type: {ex.Message}", ex);
            }
        }
    }
}