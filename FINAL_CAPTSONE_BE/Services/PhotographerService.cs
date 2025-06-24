using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class PhotographerService
    {
        private readonly IPhotographerRepository _photographerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly WeddingWonderDbContext _context;

        public PhotographerService(
            IPhotographerRepository photographerRepository,
            IUnitOfWork unitOfWork,
            WeddingWonderDbContext context)
        {
            _photographerRepository = photographerRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<List<PhotographerDTO>> GetAllPhotographersAsync()
        {
            try
            {
                var photographers = await _context.Photographers.ToListAsync();

                return photographers.Select(p => new PhotographerDTO
                {
                    PhotographerId = p.PhotographerId,
                    PhotographerName = p.PhotographerName,
                    PhotographerImage = p.PhotographerImage,
                    About = p.About,
                    Artwork1 = p.Artwork1,
                    Artwork2 = p.Artwork2,
                    ServiceId = p.ServiceId,
                    Status = p.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching photographers: {ex.Message}", ex);
            }
        }

        public async Task<PhotographerDTO?> GetPhotographerByIdAsync(int id)
        {
            try
            {
                var photographer = await _context.Photographers
                    .FirstOrDefaultAsync(p => p.PhotographerId == id);

                if (photographer == null) return null;

                return new PhotographerDTO
                {
                    PhotographerId = photographer.PhotographerId,
                    PhotographerName = photographer.PhotographerName,
                    PhotographerImage = photographer.PhotographerImage,
                    About = photographer.About,
                    Artwork1 = photographer.Artwork1,
                    Artwork2 = photographer.Artwork2,
                    ServiceId = photographer.ServiceId,
                    Status = photographer.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching photographer by id: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreatePhotographerAsync(PhotographerDTO photographerDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var photographer = new Photographer
                {
                    PhotographerName = photographerDto.PhotographerName,
                    PhotographerImage = photographerDto.PhotographerImage,
                    About = photographerDto.About,
                    Artwork1 = photographerDto.Artwork1,
                    Artwork2 = photographerDto.Artwork2,
                    ServiceId = photographerDto.ServiceId,
                    Status = photographerDto.Status
                };

                await _context.Photographers.AddAsync(photographer);
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

        public async Task<bool> UpdatePhotographerAsync(int id, PhotographerDTO photographerDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var photographer = await _context.Photographers
                    .FirstOrDefaultAsync(p => p.PhotographerId == id);

                if (photographer == null)
                    return false;

                photographer.PhotographerName = photographerDto.PhotographerName;
                photographer.PhotographerImage= photographerDto.PhotographerImage;
                photographer.About = photographerDto.About;
                photographer.Artwork1 = photographerDto.Artwork1;
                photographer.Artwork2 = photographerDto.Artwork2;
                photographer.ServiceId = photographerDto.ServiceId;
                photographer.Status = photographerDto.Status;

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

        public async Task<bool> DeletePhotographerAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _photographerRepository.DeleteAsync(id);
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
