using BusinessObject.DTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IFavoriteServiceRepository
    {
        Task<bool> AddFavoriteAsync(int userId, int serviceId);
        Task<bool> RemoveFavoriteAsync(int favoriteId, int userId);
        Task<List<FavoriteServiceDTO>> GetFavoriteServicesAsync(int userId);
        Task<FavoriteService?> GetFavoriteByUserIdAndServiceIdAsync(int userId, int serviceId);
    }
}
