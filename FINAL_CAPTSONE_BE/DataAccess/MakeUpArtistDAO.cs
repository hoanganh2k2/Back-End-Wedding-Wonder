using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    using BusinessObject.Models;
    using Microsoft.EntityFrameworkCore;

    namespace DataAccess
    {
        public class MakeUpArtistDAO
        {
            private readonly WeddingWonderDbContext _context;

            public MakeUpArtistDAO(WeddingWonderDbContext context)
            {
                _context = context;
            }

            // Lấy danh sách tất cả MakeUpArtists
            public async Task<List<MakeUpArtist>> GetMakeUpArtists()
            {
                try
                {
                    return await _context.MakeUpArtists.Include(m => m.Service).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            // Lấy thông tin MakeUpArtist theo ID
            public async Task<MakeUpArtist> GetMakeUpArtistById(int artistId)
            {
                try
                {
                    return await _context.MakeUpArtists.Include(m => m.Service)
                        .FirstOrDefaultAsync(m => m.ArtistId == artistId);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            // Tạo mới MakeUpArtist
            public async Task<bool> CreateMakeUpArtist(MakeUpArtist makeUpArtist)
            {
                try
                {
                    await _context.MakeUpArtists.AddAsync(makeUpArtist);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            // Cập nhật MakeUpArtist
            public Task UpdateMakeUpArtist(int id, MakeUpArtist makeUpArtist)
            {
                try
                {
                    _context.MakeUpArtists.Update(makeUpArtist);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }

                return Task.CompletedTask;
            }

            // Xóa MakeUpArtist
            public async Task DeleteMakeUpArtist(int artistId)
            {
                try
                {
                    var artistToDelete = await _context.MakeUpArtists.FindAsync(artistId);
                    if (artistToDelete != null)
                    {
                        _context.MakeUpArtists.Remove(artistToDelete);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("MakeUpArtist not found.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            // Lấy danh sách MakeUpArtists theo ServiceId
            public async Task<List<MakeUpArtist>> GetMakeUpArtistsByServiceId(int serviceId)
            {
                try
                {
                    return await _context.MakeUpArtists
                        .Include(m => m.Service)
                        .Where(m => m.ServiceId == serviceId)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }

}
