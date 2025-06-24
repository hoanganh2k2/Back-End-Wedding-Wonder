using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class FavoriteServiceRepository : IFavoriteServiceRepository
    {
        private readonly FavoriteServiceDAO _dao;

        public FavoriteServiceRepository(FavoriteServiceDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> AddFavoriteAsync(int userId, int serviceId)
        {
            return _dao.AddFavorite(userId, serviceId);
        }

        public Task<FavoriteService?> GetFavoriteByUserIdAndServiceIdAsync(int userId, int serviceId)
        {
            return _dao.GetFavoriteByUserIdAndServiceIdAsync(userId, serviceId);
        }

        public Task<List<FavoriteServiceDTO>> GetFavoriteServicesAsync(int userId)
        {
            return _dao.GetFavoriteServices(userId);
        }

        public Task<bool> RemoveFavoriteAsync(int favoriteId, int userId)
        {
            return _dao.RemoveFavorite(favoriteId, userId);
        }
    }
}
