using BusinessObject.Models;
using Repository.IRepositories;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class AdminLogService
    {
        private readonly IAdminLogRepository _adminLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminLogService(IAdminLogRepository adminLogRepository, IUnitOfWork unitOfWork)
        {
            _adminLogRepository = adminLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AdminLog>> GetAllLogsAsync()
        {
            return await _adminLogRepository.GetsAsync();
        }

        public async Task<AdminLog> GetLogByIdAsync(int logId)
        {
            return await _adminLogRepository.GetAsyncById(logId);
        }

        public async Task<List<AdminLog>> GetLogsByAdminIdAsync(int adminId)
        {
            return await _adminLogRepository.GetLogsByAdminId(adminId);
        }

        public async Task<bool> CreateLogAsync(AdminLog log)
        { 
            await _unitOfWork.BeginTransactionAsync();
            bool createlog = await _adminLogRepository.CreateAsync(log);  
            await _unitOfWork.CommitAsync();
            await _unitOfWork.CommitTransactionAsync();
            return createlog;
        }
        public string ExtractDeviceName(string userAgent)
        {
            if (userAgent.Contains("Windows"))
            {
                return "Windows Device";
            }
            else if (userAgent.Contains("Mac"))
            {
                return "Mac Device";
            }
            else if (userAgent.Contains("iPhone"))
            {
                return "iPhone";
            }
            else if (userAgent.Contains("Android"))
            {
                return "Android Device";
            }
            return "Unknown Device";
        }
    }
}