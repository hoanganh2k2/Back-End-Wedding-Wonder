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
    public class RequestUpgradeSupplierDAO
    {
        private readonly WeddingWonderDbContext _context;

        public RequestUpgradeSupplierDAO(WeddingWonderDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequestUpgradeSupplierDTO>> GetRequestsByStatusAsync(string status)
        {
            return await _context.RequestUpgradeSuppliers
                .Where(req => req.Status == status)
                .Include(req => req.RequestImages)
                .Select(req => new RequestUpgradeSupplierDTO
                {
                    RequestId = req.RequestId,
                    UserId = req.UserId,
                    RequestContent = req.RequestContent,
                    BusinessType = req.BusinessType,
                    Status = req.Status,
                    RejectReason = req.RejectReason,
                    CreatedAt = req.CreatedAt,
                    UpdatedAt = req.UpdatedAt,
                    IdNumber = req.IdNumber,
                    FullName = req.FullName,
                    RequestImages = req.RequestImages.Select(img => new RequestImageDTO
                    {
                        ImageId = img.ImageId,
                        RequestId = img.RequestId,
                        ImageText = img.ImageText,
                        ImageType = img.ImageType
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<RequestUpgradeSupplierDTO?> GetRequestByIdAsync(int requestId)
        {
            return await _context.RequestUpgradeSuppliers
                .Where(req => req.RequestId == requestId)
                .Select(req => new RequestUpgradeSupplierDTO
                {
                    RequestId = req.RequestId,
                    UserId = req.UserId,
                    RequestContent = req.RequestContent,
                    BusinessType = req.BusinessType,
                    Status = req.Status,
                    RejectReason = req.RejectReason,
                    CreatedAt = req.CreatedAt,
                    UpdatedAt = req.UpdatedAt,
                    IdNumber = req.IdNumber,
                    FullName = req.FullName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddRequestAsync(RequestUpgradeSupplier request)
        {
            try
            {
                await _context.RequestUpgradeSuppliers.AddAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRequestAsync(RequestUpgradeSupplier request)
        {
            try
            {
                _context.RequestUpgradeSuppliers.Update(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
