using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;

namespace Services
{
    public class MakeUpArtistService
    {
        private readonly IMakeUpArtistRepository _makeUpArtistRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly WeddingWonderDbContext _context;

        public MakeUpArtistService(
            IMakeUpArtistRepository makeUpArtistRepository,
            IUnitOfWork unitOfWork,
            WeddingWonderDbContext context)
        {
            _makeUpArtistRepository = makeUpArtistRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<List<MakeUpArtistDTO>> GetAllMakeUpArtistsAsync()
        {
            try
            {
                var artists = await _context.MakeUpArtists.ToListAsync();
                return artists.Select(a => new MakeUpArtistDTO
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName,
                    ArtistImage = a.ArtistImage,
                    Experience = a.Experience,
                    Services = a.Services,
                    Certifications = a.Certifications,
                    Awards = a.Awards,
                    ServiceId = a.ServiceId,
                    Status = a.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching makeup artists: {ex.Message}", ex);
            }
        }

        public async Task<MakeUpArtistDTO?> GetMakeUpArtistByIdAsync(int id)
        {
            try
            {
                var artist = await _context.MakeUpArtists.FirstOrDefaultAsync(a => a.ArtistId == id);
                if (artist == null) return null;

                return new MakeUpArtistDTO
                {
                    ArtistId = artist.ArtistId,
                    ArtistName = artist.ArtistName,
                    ArtistImage = artist.ArtistImage,
                    Experience = artist.Experience,
                    Services = artist.Services,
                    Certifications = artist.Certifications,
                    Awards = artist.Awards,
                    ServiceId = artist.ServiceId,
                    Status = artist.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching makeup artist by id: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateMakeUpArtistAsync(MakeUpArtistDTO artistDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var artist = new MakeUpArtist
                {
                    ArtistName = artistDto.ArtistName,
                    ArtistImage = artistDto.ArtistImage,
                    Experience = artistDto.Experience,
                    Services = artistDto.Services,
                    Certifications = artistDto.Certifications,
                    Awards = artistDto.Awards,
                    ServiceId = artistDto.ServiceId,
                    Status = artistDto.Status
                };

                await _context.MakeUpArtists.AddAsync(artist);
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

        public async Task<bool> UpdateMakeUpArtistAsync(int id, MakeUpArtistDTO artistDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var artist = await _context.MakeUpArtists.FirstOrDefaultAsync(a => a.ArtistId == id);
                if (artist == null) return false;

                artist.ArtistName = artistDto.ArtistName;
                artist.ArtistImage = artistDto.ArtistImage;
                artist.Experience = artistDto.Experience;
                artist.Services = artistDto.Services;
                artist.Certifications = artistDto.Certifications;
                artist.Awards = artistDto.Awards;
                artist.ServiceId = artistDto.ServiceId;
                artist.Status = artistDto.Status;

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

        public async Task<bool> DeleteMakeUpArtistAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _makeUpArtistRepository.DeleteAsync(id);
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
