using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;

namespace Services
{
    public class RestaurantServiceManager
    {
        private readonly ICateringRepository _cateringRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RestaurantServiceManager(
            ICateringRepository cateringRepository,
            IMenuRepository menuRepository,
            IUnitOfWork unitOfWork)
        {
            _cateringRepository = cateringRepository;
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CateringDTO>> GetAllCateringsAsync()
        {
            try
            {
                List<Catering> caterings = await _cateringRepository.GetsAsync();
                return caterings.Select(c => new CateringDTO
                {
                    CateringId = c.CateringId,
                    StyleName = c.StyleName,
                    PackageContent = c.PackageContent,
                    CateringImage = c.CateringImage,
                    Status = c.Status,
                    ServiceId = c.ServiceId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<CateringDTO?> GetCateringByIdAsync(int id)
        {
            try
            {
                Catering catering = await _cateringRepository.GetAsyncById(id);
                if (catering == null) return null;

                return new CateringDTO
                {
                    CateringId = catering.CateringId,
                    StyleName = catering.StyleName,
                    PackageContent = catering.PackageContent,
                    CateringImage = catering.CateringImage,
                    Status = catering.Status,
                    ServiceId = catering.ServiceId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<CateringDTO>> GetCateringPackagesByServiceIdAsync(int serviceid)
        {
            try
            {
                List<Catering> packages = await _cateringRepository.GetAsyncByServiceId(serviceid);

                if (packages == null || !packages.Any())
                    return new List<CateringDTO>();

                List<CateringDTO> packageDTOs = packages.Select(package => new CateringDTO
                {
                    CateringId = package.CateringId,
                    StyleName = package.StyleName,
                    PackageContent = package.PackageContent,
                    CateringImage = package.CateringImage,
                    Status = package.Status,
                    ServiceId = package.ServiceId
                }).ToList();

                return packageDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<decimal?> GetPriceByPackageId(int id, int number)
        {
            try
            {
                Menu package = await _menuRepository.GetAsyncById(id);
                if (package == null) return 0;

                int table = (int)Math.Ceiling((double)number / 8);

                return package.Price * table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateCateringAsync(CateringDTO cateringDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Catering catering = new()
                {
                    StyleName = cateringDto.StyleName,
                    PackageContent = cateringDto.PackageContent,
                    CateringImage = cateringDto.CateringImage,
                    Status = cateringDto.Status,
                    ServiceId = cateringDto.ServiceId
                };

                await _cateringRepository.CreateAsync(catering);
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
        public async Task<bool> UpdateCateringAsync(int id, CateringDTO cateringDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Catering existingCatering = await _cateringRepository.GetAsyncById(id);
                if (existingCatering == null) return false;

                existingCatering.StyleName = cateringDto.StyleName;
                existingCatering.PackageContent = cateringDto.PackageContent;
                existingCatering.CateringImage = cateringDto.CateringImage;
                existingCatering.Status = cateringDto.Status;
                existingCatering.ServiceId = cateringDto.ServiceId;

                await _cateringRepository.UpdateAsync(id, existingCatering);
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
        public async Task<bool> DeleteCateringAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _cateringRepository.DeleteAsync(id);
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
