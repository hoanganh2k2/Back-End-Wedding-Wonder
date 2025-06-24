using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ServiceDAO
    {
        private readonly WeddingWonderDbContext context;

        public ServiceDAO(WeddingWonderDbContext _context)
        {
            context = _context;
        }

        public async Task<List<Service>> GetServices()
        {
            try
            {
                return await context.Services
                   .AsNoTracking()
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .Where(s => s.IsActive == 1)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetServicesByType(int typeid)
        {
            try
            {
                return await context.Services
                   .Where(u => u.ServiceTypeId == typeid && u.IsActive == 1)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetServicesByUserIdAndType(int userId, int serviceTypeId)
        {
            try
            {
                return await context.Services
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .Where(s => s.Supplier.UserId == userId && s.ServiceTypeId == serviceTypeId && s.IsActive == 1)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Service> GetServiceById(int serviceId)
        {
            try
            {
                return await context.Services
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.IsActive == 1) ?? throw new Exception("Service not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateService(Service service)
        {
            try
            {
                service.IsActive = 2; // Pending approval
                await context.Services.AddAsync(service);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateService(int id, Service service)
        {
            try
            {
                Service? existingService = await context.Services.FindAsync(id);
                if (existingService == null)
                    throw new Exception("Service not found.");

                context.Entry(existingService).CurrentValues.SetValues(service);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteService(int serviceId)
        {
            try
            {
                Service? serviceToDelete = await context.Services.FindAsync(serviceId);
                if (serviceToDelete != null)
                {
                    serviceToDelete.IsActive = 0; // Soft delete
                }
                else
                {
                    throw new Exception("Service not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Service> AcceptService(int serviceId)
        {
            try
            {
                Service? serviceToAccept = await context.Services.FindAsync(serviceId);
                if (serviceToAccept != null)
                {
                    serviceToAccept.IsActive = 1; // Active
                }
                else
                {
                    throw new Exception("Service not found.");
                }
                return serviceToAccept;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Service> RejectService(int serviceId)
        {
            try
            {
                var serviceToReject = await context.Services.FindAsync(serviceId);
                if (serviceToReject != null)
                {
                    context.Services.Remove(serviceToReject);
                }
                else
                {
                    throw new Exception("Service not found.");
                }
                return serviceToReject;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from ServiceDAO(AcceptService): " + ex.Message, ex);
            }
        }

        public async Task<List<Service>> SearchServices(string keyword, int? serviceTypeId, string city)
        {
            try
            {
                IQueryable<Service> query = context.Services.AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(s => EF.Functions.Like(s.ServiceName ?? "", $"%{keyword}%") || EF.Functions.Like(s.Description ?? "", $"%{keyword}%"));

                if (serviceTypeId.HasValue)
                    query = query.Where(s => s.ServiceTypeId == serviceTypeId.Value);

                if (!string.IsNullOrEmpty(city))
                    query = query.Where(s => EF.Functions.Like(s.City ?? "", $"%{city}%"));

                return await query
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetPopularServices(int count)
        {
            try
            {
                return await context.Services
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .Where(s => s.IsActive == 1 && s.StarNumber > 0)
                    .OrderByDescending(s => s.StarNumber)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<object> GetServiceStatistics()
        {
            try
            {
                int totalServices = await context.Services.CountAsync();
                int activeServices = await context.Services.CountAsync(s => s.IsActive == 1);
                int pendingServices = await context.Services.CountAsync(s => s.IsActive == 2);
                int serviceTypes = await context.ServiceTypes.CountAsync();

                return new
                {
                    TotalServices = totalServices,
                    ActiveServices = activeServices,
                    PendingServices = pendingServices,
                    ServiceTypes = serviceTypes
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<object> GetStatisticsBySupplierId(int supplierId)
        {
            try
            {
                int totalServices = await context.Services
                    .Where(s => s.SupplierId == supplierId)
                    .CountAsync();

                int activeServices = await context.Services
                    .Where(s => s.SupplierId == supplierId && s.IsActive == 1)
                    .CountAsync();

                int pendingServices = await context.Services
                    .Where(s => s.SupplierId == supplierId && s.IsActive == 2)
                    .CountAsync();

                int serviceTypes = await context.Services
                    .Where(s => s.SupplierId == supplierId)
                    .Select(s => s.ServiceTypeId)
                    .Distinct()
                    .CountAsync();

                return new
                {
                    TotalServices = totalServices,
                    ActiveServices = activeServices,
                    PendingServices = pendingServices,
                    ServiceTypes = serviceTypes
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetRelatedServices(int serviceId, int count)
        {
            try
            {
                Service? service = await context.Services.FindAsync(serviceId);
                if (service == null)
                    throw new Exception("Service not found.");

                return await context.Services
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .Where(s => s.ServiceTypeId == service.ServiceTypeId && s.ServiceId != serviceId && s.IsActive == 1)
                    .OrderByDescending(s => s.StarNumber)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetServicesBySupplier(int supplierId)
        {
            try
            {
                return await context.Services
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .Where(s => s.SupplierId == supplierId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetPendingApprovalServices()
        {
            try
            {
                return await context.Services
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .Include(s => s.ServiceImages)
                    .Where(s => s.IsActive == 2)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateStar(int serviceId, decimal starNew)
        {
            try
            {
                Service? serviceToUpdate = await context.Services.FindAsync(serviceId);
                if (serviceToUpdate != null)
                {
                    serviceToUpdate.StarNumber = starNew;
                    context.Services.Update(serviceToUpdate);
                }
                else
                {
                    throw new Exception("Service not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CheckSupplierAndService(int supplierId, int serviceId)
        {
            try
            {
                Service? service = await context.Services.FindAsync(serviceId);
                if (service == null) throw new Exception("Service not found");

                return service.SupplierId == supplierId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> AddServiceTopics(int serviceId, List<int> topicIds)
        {
            try
            {
                foreach (var topicId in topicIds)
                {
                    var serviceTopic = new ServiceTopic
                    {
                        ServiceId = serviceId,
                        TopicId = topicId,
                        CreatedAt = DateTime.Now
                    };

                    await context.ServiceTopics.AddAsync(serviceTopic);
                }
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> FilterServices(int[] serviceTypeIds, string city, DateTime? freeScheduleDate, int[] starNumbers)
        {
            try
            {
                IQueryable<Service> query = context.Services.AsQueryable();

                // condition for serviceTypeIds
                if (serviceTypeIds != null && serviceTypeIds.Length > 0)
                {
                    query = query.Where(s => serviceTypeIds.Contains(s.ServiceTypeId));
                }

                // condition for city
                if (!string.IsNullOrEmpty(city))
                {
                    query = query.Where(s => EF.Functions.Like(s.City ?? "", $"%{city}%"));
                }

                // condition for FreeScheduleDate
                if (freeScheduleDate.HasValue)
                {
                    var busySchedules = await context.BusySchedules
                        .Where(bs => bs.StartDate <= freeScheduleDate.Value && bs.EndDate >= freeScheduleDate.Value)
                        .Select(bs => bs.ServiceId)
                        .ToListAsync();

                    query = query.Where(s => !busySchedules.Contains(s.ServiceId));
                }

                // condition for starNumbers
                if (starNumbers != null && starNumbers.Length > 0)
                {
                    query = query.Where(s => starNumbers.Contains((int)(s.StarNumber ?? 0)));
                }

                return await query
                    .Include(s => s.Supplier)
                    .Include(s => s.ServiceType)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}