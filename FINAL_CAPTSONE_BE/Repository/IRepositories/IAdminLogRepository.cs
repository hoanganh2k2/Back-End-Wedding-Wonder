using BusinessObject.Models;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IAdminLogRepository : IRepository<AdminLog>
    {
        Task<List<AdminLog>> GetLogsByAdminId(int adminId);
        Task<List<AdminLog>> GetAllLogs();   
        Task<AdminLog> GetLogById(int logId);   
    }
}