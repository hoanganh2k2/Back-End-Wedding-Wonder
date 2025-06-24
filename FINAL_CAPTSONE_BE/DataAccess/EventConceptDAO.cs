using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class EventConceptDAO
    {
        private readonly WeddingWonderDbContext context;

        public EventConceptDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<EventConcept>> GetEventConcepts()
        {
            try
            {
                return await context.EventConcepts
                    .Where(c => c.Status == true) 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EventConcept> GetEventConceptById(int conceptId)
        {
            try
            {
                EventConcept? concept = await context.EventConcepts.FindAsync(conceptId);

                return concept ?? throw new Exception("EventConcept not found.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<EventConcept>> GetEventConceptsByPackageId(int packageId)
        {
            try
            {
                return await context.EventConcepts
                    .Where(c => c.PackageId == packageId && c.Status == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateEventConcept(EventConcept concept)
        {
            try
            {
                concept.Status = true;  
                await context.EventConcepts.AddAsync(concept);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateEventConcept(int conceptId, EventConcept concept)
        {
            try
            {
                EventConcept? existingConcept = await context.EventConcepts.FindAsync(conceptId);
                if (existingConcept == null)
                    throw new Exception("EventConcept not found.");

                context.Entry(existingConcept).CurrentValues.SetValues(concept);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteEventConcept(int conceptId)
        {
            try
            {
                EventConcept? conceptToDelete = await context.EventConcepts.FindAsync(conceptId);
                if (conceptToDelete != null)
                {
                    conceptToDelete.Status = false; 
                    context.EventConcepts.Update(conceptToDelete);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("EventConcept not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
