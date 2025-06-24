using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EventPackageDAO
    {
        private readonly WeddingWonderDbContext context;

        public EventPackageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<EventPackage>> GetEventPackages()
        {
            try
            {
                List<EventPackage> eventPackages = await context.EventPackages
                        .ToListAsync();

                return eventPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<EventPackage> GetEventPackageById(int eventPackageId)
        {
            try
            {
                EventPackage? eventPackage = await context.EventPackages.FindAsync(eventPackageId);

                return eventPackage ?? throw new Exception("EventPackage not found.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<EventPackage> GetEventByConceptId(int conceptId)
        {
            try
            {
                EventConcept? concept = await context.EventConcepts.Include(m => m.Package)
                            .FirstOrDefaultAsync(m => m.ConceptId == conceptId);

                if (concept == null)
                {
                    throw new Exception("Concept not found");
                }

                return concept.Package;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<EventPackage>> GetEventPackagesByServiceId(int serviceId)
        {
            try
            {
                List<EventPackage> eventPackages = await context.EventPackages
                    .Where(s => s.ServiceId == serviceId && s.Status == true)
                    .ToListAsync();

                return eventPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Boolean> CreateEventPackage(EventPackage eventPackage)
        {
            try
            {
                eventPackage.Status = true;
                await context.EventPackages.AddAsync(eventPackage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateEventPackage(int id, EventPackage eventPackage)
        {
            try
            {
                EventPackage? existingPackage = await context.EventPackages.FindAsync(id);
                if (existingPackage == null)
                    throw new Exception("EventPackage not found.");

                context.Entry(existingPackage).CurrentValues.SetValues(eventPackage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteEventPackage(int eventPackageId)
        {
            try
            {
                EventPackage? eventPackageToDelete = await context.EventPackages.FindAsync(eventPackageId);
                if (eventPackageToDelete != null)
                {
                    eventPackageToDelete.Status = false;
                    context.EventPackages.Update(eventPackageToDelete);
                }
                else
                {
                    throw new Exception("EventPackage not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<EventPackage>> GetEventPackagesByEventType(int eventType)
{
    try
    {
        return await context.EventPackages
            .Where(p => p.EventType == eventType && p.Status == true) 
            .ToListAsync();
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message, ex);
    }
}
        public async Task<List<EventPackage>> GetEventPackagesByEventTypeAndServiceId(int eventType, int serviceId)
        {
            try
            {
                return await context.EventPackages
                    .Where(p => p.EventType == eventType && p.ServiceId == serviceId && p.Status == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}