using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AdminLogDAO
    {
        private readonly WeddingWonderDbContext context;

        public AdminLogDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<AdminLog>> GetAllLogs()
        {
            try
            {
                return await context.AdminLogs
                   .OrderByDescending(log => log.CreatedAt)  
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching logs: " + ex.Message, ex);
            }
        }

        public async Task<AdminLog> GetLogById(int logId)
        {
            try
            {
                var log = await context.AdminLogs.FindAsync(logId);
                if (log == null)
                {
                    throw new Exception("Log not found");
                }

                return log;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching log by ID: " + ex.Message, ex);
            }
        }

        public async Task<List<AdminLog>> GetLogsByAdminId(int adminId)
        {
            try
            {
                return await context.AdminLogs
                    .Where(log => log.AdminId == adminId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching logs by Admin ID: " + ex.Message, ex);
            }
        }

        public async Task<bool> CreateLog(AdminLog log)
        {
            try
            {
                await context.AdminLogs.AddAsync(log); 
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating log: " + ex.Message, ex);
            }
        } 
    }
}
