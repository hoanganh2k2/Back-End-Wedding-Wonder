using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PhotographerDAO
    {
        private readonly WeddingWonderDbContext context;

        public PhotographerDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Photographer>> GetPhotographers()
        {
            try
            {
                return await context.Photographers.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Photographer> GetPhotographerById(int photographerId)
        {
            try
            {
                return await context.Photographers.FirstOrDefaultAsync(p => p.PhotographerId == photographerId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreatePhotographer(Photographer photographer)
        {
            try
            {
                await context.Photographers.AddAsync(photographer);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public Task UpdatePhotographer(int id, Photographer photographer)
        {
            try
            {
                context.Photographers.Update(photographer);
                return context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeletePhotographer(int photographerId)
        {
            try
            {
                var photographerToDelete = await context.Photographers.FindAsync(photographerId);
                if (photographerToDelete != null)
                {
                    context.Photographers.Remove(photographerToDelete);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Photographer not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
