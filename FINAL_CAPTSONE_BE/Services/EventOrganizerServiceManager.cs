using BusinessObject.Models;
using Repository.IRepositories;
using WeddingWonderAPI.Models.DTOs;

namespace Services
{
    public class EventOrganizeServiceManager
    {
        private readonly IEventPackageRepository _eventPackageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EventOrganizeServiceManager(
            IEventPackageRepository eventPackageRepository,
            IUnitOfWork unitOfWork)
        {
            _eventPackageRepository = eventPackageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EventPackageDTO>> GetAllEventPackagesAsync()
        {
            try
            {
                List<EventPackage> packages = await _eventPackageRepository.GetsAsync();
                return packages.Select(p => new EventPackageDTO
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    ServiceId = p.ServiceId,
                    PackagePrice = p.PackagePrice ?? 0m,
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

        public async Task<EventPackageDTO?> GetEventPackageByIdAsync(int id)
        {
            try
            {
                EventPackage package = await _eventPackageRepository.GetAsyncById(id);
                if (package == null) return null;

                return new EventPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0m,
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
                EventPackage package = await _eventPackageRepository.GetAsyncById(id);
                if (package == null) return 0;

                return package.PackagePrice;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<EventPackageDTO>> GetEventPackagesByServiceIdAsync(int serviceid)
        {
            try
            {
                List<EventPackage> packages = await _eventPackageRepository.GetAsyncByServiceId(serviceid);

                if (packages == null || !packages.Any()) return new List<EventPackageDTO>();

                List<EventPackageDTO> packageDTOs = packages.Select(package => new EventPackageDTO
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
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateEventPackageAsync(EventPackageDTO eventPackageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                EventPackage package = new()
                {
                    PackageName = eventPackageDto.PackageName,
                    PackagePrice = eventPackageDto.PackagePrice,
                    ServiceId = eventPackageDto.ServiceId,
                    PackageContent = eventPackageDto.PackageContent,
                    EventType = eventPackageDto.EventType,
                    Status = eventPackageDto.Status
                };

                await _eventPackageRepository.CreateAsync(package);
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

        public async Task<bool> UpdateEventPackageAsync(int id, EventPackageDTO packageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                EventPackage existingPackage = await _eventPackageRepository.GetAsyncById(id);
                if (existingPackage == null) return false;

                existingPackage.PackageName = packageDto.PackageName;
                existingPackage.PackagePrice = packageDto.PackagePrice;
                existingPackage.PackageContent = packageDto.PackageContent;
                existingPackage.EventType = packageDto.EventType;
                existingPackage.Status = packageDto.Status;

                await _eventPackageRepository.UpdateAsync(id, existingPackage);
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

        public async Task<bool> DeleteEventPackageAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _eventPackageRepository.DeleteAsync(id);
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

        public async Task<List<EventPackageDTO>> GetEventPackagesByEventTypeAsync(int eventType)
        {
            try
            {
                List<EventPackage> packages = await _eventPackageRepository.GetEventPackagesByEventType(eventType);

                if (packages == null || !packages.Any()) return new List<EventPackageDTO>();

                return packages.Select(package => new EventPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageContent = package.PackageContent,
                    EventType = package.EventType,
                    Status = package.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<EventPackageDTO>> GetEventPackagesByEventTypeAndServiceId(int eventType, int serviceId)
        {
            try
            {
                List<EventPackage> packages = await _eventPackageRepository.GetEventPackagesByEventTypeAndServiceId(eventType, serviceId);

                if (packages == null || !packages.Any()) return new List<EventPackageDTO>();

                return packages.Select(package => new EventPackageDTO
                {
                    PackageId = package.PackageId,
                    PackageName = package.PackageName,
                    ServiceId = package.ServiceId,
                    PackagePrice = package.PackagePrice ?? 0,
                    PackageContent = package.PackageContent,
                    EventType = package.EventType,
                    Status = package.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}