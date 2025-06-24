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
    public class RequestImageDAO
    {
        private readonly WeddingWonderDbContext _context;

        public RequestImageDAO(WeddingWonderDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequestImageDTO>> GetImagesByRequestIdAsync(int requestId)
        {
            return await _context.RequestImages
                .Where(img => img.RequestId == requestId)
                .Select(img => new RequestImageDTO
                {
                    ImageId = img.ImageId,
                    RequestId = img.RequestId,
                    ImageText = img.ImageText,
                    ImageType = img.ImageType
                })
                .ToListAsync();
        }

        public async Task<bool> AddImagesAsync(List<RequestImage> images)
        {
            try
            {
                await _context.RequestImages.AddRangeAsync(images);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
