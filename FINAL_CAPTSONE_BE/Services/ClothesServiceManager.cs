using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.Extensions.Configuration;
using Repository.IRepositories;

namespace Services
{
    public class ClothesServiceManager
    {
        private readonly IOutfitRepository _outfitRepository;
        private readonly IOutfitOutfitTypeRepository _typeRepository;
        private readonly IOutfitImageRepository _imageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ClothesServiceManager(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IOutfitRepository outfitRepository,
            IOutfitImageRepository imageRepository,
            IOutfitOutfitTypeRepository typeRepository)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _outfitRepository = outfitRepository;
            _imageRepository = imageRepository;
            _typeRepository = typeRepository;
        }

        public async Task<List<OutfitDTO>> GetAllOutfitsOfStore(int id)
        {
            try
            {
                List<Outfit> outfits = await _outfitRepository.GetOutfitsOfStore(id);

                List<OutfitDTO> outfitDTOs = new();

                foreach (Outfit o in outfits)
                {
                    OutfitImage? images = await _imageRepository.GetFirstImageOfOutfit(o.OutfitId);
                    string avatarImage = images?.ImageText ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg";

                    outfitDTOs.Add(new OutfitDTO
                    {
                        OutfitId = o.OutfitId,
                        OutfitName = o.OutfitName,
                        OutfitPrice = o.OutfitPrice,
                        AvatarImage = avatarImage,
                        OutfitDescription = o.OutfitDescription,
                        ServiceId = o.ServiceId,
                        OutfitTypes = await _typeRepository.GetListsTypeOutfit(o.OutfitId),
                        Status = o.Status
                    });

                }

                return outfitDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<OutfitDTO>> GetOutfitsByTypeAndServiceId(int id, int type)
        {
            try
            {
                List<Outfit> outfits = await _outfitRepository.GetOutfitsByTypeOfstore(type, id);

                List<OutfitDTO> outfitDTOs = new();

                foreach (Outfit o in outfits)
                {
                    OutfitImage? images = await _imageRepository.GetFirstImageOfOutfit(o.OutfitId);
                    string avatarImage = images?.ImageText ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg";

                    outfitDTOs.Add(new OutfitDTO
                    {
                        OutfitId = o.OutfitId,
                        OutfitName = o.OutfitName,
                        OutfitPrice = o.OutfitPrice,
                        AvatarImage = avatarImage,
                        OutfitDescription = o.OutfitDescription,
                        ServiceId = o.ServiceId,
                        OutfitTypes = await _typeRepository.GetListsTypeOutfit(o.OutfitId),
                        Status = o.Status
                    });

                }

                return outfitDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<OutfitDTO> GetOutfitById(int outfitId)
        {
            try
            {
                Outfit outfits = await _outfitRepository.GetAsyncById(outfitId);

                OutfitDTO outfitDTO = new()
                {
                    OutfitId = outfits.OutfitId,
                    OutfitName = outfits.OutfitName,
                    OutfitPrice = outfits.OutfitPrice,
                    OutfitDescription = outfits.OutfitDescription,
                    ServiceId = outfits.ServiceId,
                    Status = outfits.Status
                };
                OutfitImage? image = await _imageRepository.GetFirstImageOfOutfit(outfitId);
                string avatarImage = image?.ImageText ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg";
                List<OutfitImage> images = await _imageRepository.GetImagesByOutfitId(outfitId);

                outfitDTO.AvatarImage = avatarImage;
                outfitDTO.AllImage = images.Select(i => i.ImageText).ToList();
                outfitDTO.OutfitTypes = await _typeRepository.GetListsTypeOutfit(outfitId);

                return outfitDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<OutfitDTO>> GetOutfitPackagesByServiceIdAsync(int serviceId)
        {
            try
            {
                return await GetAllOutfitsOfStore(serviceId);

                /*                List<Outfit> packages = await _outfitRepository.GetAsyncByServiceId(serviceId);

                                if (packages == null || !packages.Any()) return new List<OutfitDTO>();

                                List<OutfitDTO> packageDTOs = packages.Select(package => new OutfitDTO
                                {
                                    OutfitId = package.OutfitId,
                                    OutfitName = package.OutfitName,
                                    OutfitPrice = package.OutfitPrice,
                                    OutfitDescription = package.OutfitDescription,
                                    ServiceId = package.ServiceId,
                                    Status = package.Status
                                }).ToList();

                                return packageDTOs;*/
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
        public async Task UpdateOutfit(int id, OutfitDTO outfitDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Outfit outfit = new()
                {
                    OutfitName = outfitDTO.OutfitName ?? "",
                    OutfitDescription = outfitDTO.OutfitDescription,
                    OutfitPrice = outfitDTO.OutfitPrice,
                    Status = outfitDTO.Status
                };
                await _outfitRepository.UpdateAsync(id, outfit);

                List<int> typeInDb = await _typeRepository.GetListsTypeOutfit(id);
                if (outfitDTO.OutfitTypes != typeInDb)
                {
                    foreach (int i in typeInDb) await _typeRepository.DeleteType(id, i);
                    await _unitOfWork.CommitAsync();
                    foreach (int i in outfitDTO.OutfitTypes)
                    {
                        OutfitOutfitType type = new()
                        {
                            OutfitId = id,
                            OutfitTypeId = i
                        };
                        await _typeRepository.CreateAsync(type);
                    }
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteOutfit(int outfitId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _outfitRepository.DeleteAsync(outfitId);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateOutfit(OutfitDTO outfitDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Outfit outfit = new()
                {
                    OutfitName = outfitDTO.OutfitName ?? "Outfit",
                    OutfitDescription = outfitDTO.OutfitDescription,
                    OutfitPrice = outfitDTO.OutfitPrice,
                    ServiceId = outfitDTO.ServiceId ?? 0,
                    Status = 1
                };
                bool status = await _outfitRepository.CreateAsync(outfit);
                await _unitOfWork.CommitAsync();

                foreach (int i in outfitDTO.OutfitTypes)
                {
                    OutfitOutfitType type = new()
                    {
                        OutfitId = outfit.OutfitId,
                        OutfitTypeId = i
                    };
                    await _typeRepository.CreateAsync(type);
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckOutfitId(int outfitId, int supplierId)
        {
            try
            {
                return await _outfitRepository.CheckOutfit(outfitId, supplierId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
