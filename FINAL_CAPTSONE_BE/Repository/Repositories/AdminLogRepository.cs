using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AdminLogRepository : IAdminLogRepository
    {
        private readonly AdminLogDAO _dao;

        public AdminLogRepository(AdminLogDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(AdminLog obj) => _dao.CreateLog(obj); 

        public Task<AdminLog> GetAsyncById(int id) => _dao.GetLogById(id);

        public Task<List<AdminLog>> GetsAsync() => _dao.GetAllLogs();   
         
        public Task<List<AdminLog>> GetLogsByAdminId(int adminId) => _dao.GetLogsByAdminId(adminId);

        public Task<List<AdminLog>> GetAllLogs() => _dao.GetAllLogs();

        public Task<AdminLog> GetLogById(int logId) => _dao.GetLogById(logId);   

        public Task UpdateAsync(int id, AdminLog obj)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}