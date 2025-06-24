using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EventOrganizerServiceDAO
    {
        private readonly WeddingWonderDbContext _context;

        public EventOrganizerServiceDAO(WeddingWonderDbContext context)
        {
            _context = context;
        }

        public async Task<List<EventOrganizerService>> GetEventOrganizerServices()
        {
            try
            {
                List<EventOrganizerService> eventOrganizerServices = await _context.EventOrganizerServices
                    .Include(e => e.Service)
                    .Include(e => e.EventPackages)
                    .ToListAsync();

                return eventOrganizerServices;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<EventOrganizerService> GetEventOrganizerServiceById(int id)
        {
            try
            {
                var service = await _context.EventOrganizerServices
                    .Include(e => e.Service)
                    .Include(e => e.EventPackages)
                    .FirstOrDefaultAsync(e => e.ServiceId == id);

                return service ?? throw new Exception($"EventOrganizerService with id {id} not found.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateEventOrganizerService(EventOrganizerService eventOrganizerService)
        {
            try
            {
                await _context.EventOrganizerServices.AddAsync(eventOrganizerService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateEventOrganizerService(int id, EventOrganizerService eventOrganizerService)
        {
            try
            {
                var existingService = await _context.EventOrganizerServices
                    .Include(e => e.EventPackages)
                    .FirstOrDefaultAsync(e => e.ServiceId == id);

                if (existingService == null)
                {
                    throw new Exception("EventOrganizerService not found.");
                }

                _context.EventOrganizerServices.Update(existingService);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}