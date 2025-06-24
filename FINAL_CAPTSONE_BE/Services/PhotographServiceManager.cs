using BusinessObject.Models;
using Repository.IRepositories;
using WeddingWonderAPI.Models.DTOs;

namespace Services
{
    public class PhotographServiceManager
    {
        private readonly IPhotographPackageRepository _photographPackageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PhotographServiceManager(
            IPhotographPackageRepository photographPackageRepository,
            IUnitOfWork unitOfWork)
        {
            _photographPackageRepository = photographPackageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PhotographPackageDTO>> GetAllPhotographPackagesAsync()
        {
            try
            {
                List<PhotographPackage> packages = await _photographPackageRepository.GetsAsync();
                return packages.Select(p => new PhotographPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0,
                    PackageContent = p.PackageContent,
                    EventType = p.EventType,
                    PhotoType = p.PhotoType,
                    Location = p.Location,
                    ImageSamples = p.ImageSamples,
                    Status = p.Status,

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PhotographPackageDTO?> GetPhotographPackagesByIdAsync(int id)
        {
            try
            {
                PhotographPackage package = await _photographPackageRepository.GetAsyncById(id);
                if (package == null) return null;

                return new PhotographPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageContent = package.PackageContent,
                    EventType = package.EventType,
                    PhotoType = package.PhotoType,
                    Location = package.Location,
                    ImageSamples = package.ImageSamples,
                    Status = package.Status,

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<PhotographPackageDTO>> GetPhotoPackagesByServiceIdAsync(int serviceid)
        {
            try
            {
                List<PhotographPackage> packages = await _photographPackageRepository.GetAsyncByServiceId(serviceid);

                if (packages == null || !packages.Any()) return new List<PhotographPackageDTO>();

                List<PhotographPackageDTO> packageDTOs = packages.Select(package => new PhotographPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageContent = package.PackageContent,
                    EventType = package.EventType,
                    PhotoType = package.PhotoType,
                    Location = package.Location,
                    ImageSamples = package.ImageSamples,
                    Status = package.Status,
                }).ToList();

                return packageDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting make-up packages by service id: {ex.Message}", ex);
            }
        }

        public async Task<decimal?> GetPriceByPackageId(int preId, int weddingId)
        {
            try
            {
                PhotographPackage pre = await _photographPackageRepository.GetAsyncById(preId);
                PhotographPackage wedding = await _photographPackageRepository.GetAsyncById(weddingId);

                decimal prePrice = pre?.PackagePrice ?? 0;
                decimal weddingPrice = wedding?.PackagePrice ?? 0;


                return prePrice + weddingPrice;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreatePhotographPackagesAsync(PhotographPackageDTO photographPackageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                PhotographPackage package = new()
                {
                    PackageName = photographPackageDto.PackageName,
                    PackagePrice = photographPackageDto.PackagePrice,
                    ServiceId = photographPackageDto.ServiceId,
                    PackageContent = photographPackageDto.PackageContent,
                    EventType = photographPackageDto.EventType,
                    PhotoType = photographPackageDto.PhotoType,
                    Location = photographPackageDto.Location,
                    ImageSamples = photographPackageDto.ImageSamples,
                    Status = photographPackageDto.Status

                };

                await _photographPackageRepository.CreateAsync(package);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Exception? innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine($"Inner Exception: {innerException.Message}");
                    Console.WriteLine($"Inner Exception Stack Trace: {innerException.StackTrace}");
                    innerException = innerException.InnerException;
                }
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdatePhotographPackagesAsync(int id, PhotographPackageDTO packageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                PhotographPackage existingPackage = await _photographPackageRepository.GetAsyncById(id);
                if (existingPackage == null) return false;

                existingPackage.PackageName = packageDto.PackageName;
                existingPackage.PackagePrice = packageDto.PackagePrice;
                existingPackage.PackageContent = packageDto.PackageContent;
                existingPackage.EventType = packageDto.EventType;
                existingPackage.Status = packageDto.Status;
                existingPackage.PhotoType = packageDto.PhotoType;
                existingPackage.Location = packageDto.Location;
                existingPackage.ImageSamples = packageDto.ImageSamples;


                await _photographPackageRepository.UpdateAsync(id, existingPackage);
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

        public async Task<bool> DeletePhotographPackagesAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _photographPackageRepository.DeleteAsync(id);
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

        public async Task<List<PhotographPackageDTO>> GetPhotographPackagesByEventTypeAsync(int eventType)
        {
            try
            {
                List<PhotographPackage> packages = await _photographPackageRepository.GetAsyncByEventType(eventType);
                return packages.Select(p => new PhotographPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0,
                    PackageContent = p.PackageContent,
                    EventType = p.EventType,
                    Status = p.Status,
                    PhotoType = p.PhotoType,
                    Location = p.Location,
                    ImageSamples = p.ImageSamples

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<PhotographPackageDTO>> GetPhotographPackagesByEventTypeAndServiceIdAsync(int eventType, int serviceId)
        {
            try
            {
                List<PhotographPackage> packages = await _photographPackageRepository.GetAsyncByEventTypeAndServiceId(eventType, serviceId);

                if (packages == null || !packages.Any())
                    return new List<PhotographPackageDTO>();

                return packages.Select(p => new PhotographPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0,
                    PackageContent = p.PackageContent,
                    EventType = p.EventType,
                    Status = p.Status,
                    PhotoType = p.PhotoType,
                    Location = p.Location,
                    ImageSamples = p.ImageSamples

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving packages by event type and serviceId: {ex.Message}", ex);
            }
        }
    }
}