using BusinessObject.DTOs;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingWonderAPI.Models;

namespace Services
{
    public class FavoriteServiceService
    {
        private readonly IFavoriteServiceRepository _favoriteServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteServiceService(IFavoriteServiceRepository favoriteServiceRepository, IUnitOfWork unitOfWork)
        {
            _favoriteServiceRepository = favoriteServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddFavoriteAsync(int userId, int serviceId)
        {
            try
            {
                var existingFavorite = await _favoriteServiceRepository.GetFavoriteByUserIdAndServiceIdAsync(userId, serviceId);
                if (existingFavorite != null)
                {
                    throw new Exception("Service is already in favorites.");
                }

                await _unitOfWork.BeginTransactionAsync();
                bool isAdded = await _favoriteServiceRepository.AddFavoriteAsync(userId, serviceId);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return isAdded;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error adding to favorites: " + ex.Message, ex);
            }
        }

        public async Task<bool> RemoveFavoriteAsync(int favoriteId, int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                bool isRemoved = await _favoriteServiceRepository.RemoveFavoriteAsync(favoriteId, userId);
                if (isRemoved)
                {
                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                    return true;
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<FavoriteServiceDTO>> GetFavoriteServicesAsync(int userId)
        {
            return await _favoriteServiceRepository.GetFavoriteServicesAsync(userId);
        }

    }
}
