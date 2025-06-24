using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FavoriteServiceDAO
    {
        private readonly WeddingWonderDbContext _context;

        public FavoriteServiceDAO(WeddingWonderDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFavorite(int userId, int serviceId)
        {
            try
            {
                var favorite = new FavoriteService { UserId = userId, ServiceId = serviceId };
                await _context.FavoriteServices.AddAsync(favorite);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding favorite service: " + ex.Message, ex);
            }
        }

        public async Task<bool> RemoveFavorite(int favoriteId, int userId)
        {
            try
            {
                var favorite = await _context.FavoriteServices
                    .FirstOrDefaultAsync(f => f.FavoriteId == favoriteId && f.UserId == userId);
                if (favorite != null)
                {
                    _context.FavoriteServices.Remove(favorite);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing favorite service: " + ex.Message, ex);
            }
        }

        public async Task<List<FavoriteServiceDTO>> GetFavoriteServices(int userId)
        {
            try
            {
                var favoriteServices = await _context.FavoriteServices
                    .Where(f => f.UserId == userId)
                    .Include(f => f.Service)   
                        .ThenInclude(s => s.ServiceImages) 
                    .ToListAsync();

                return favoriteServices.Select(f => new FavoriteServiceDTO
                {
                    FavoriteId = f.FavoriteId,
                    CreatedAt = (DateTime)f.CreatedAt,
                    ServiceId = f.Service.ServiceId,
                    ServiceName = f.Service.ServiceName,
                    SupplierId = f.Service.SupplierId,
                    ServiceTypeId = f.Service.ServiceTypeId,
                    StarNumber = (int)Math.Round(f.Service.StarNumber ?? 0),
                    City = f.Service.City,
                    District = f.Service.District,
                    Ward = f.Service.Ward,
                    Address = f.Service.Address,
                    Description = f.Service.Description,
                    VisitWebsiteLink = f.Service.VisitWebsiteLink,
                    Images = f.Service.ServiceImages.Select(img => img.ImageText).ToList()  
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving favorite services: " + ex.Message, ex);
            }
        }
        public async Task<FavoriteService?> GetFavoriteByUserIdAndServiceIdAsync(int userId, int serviceId)
        {
            return await _context.FavoriteServices
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ServiceId == serviceId);
        }
    }
}
