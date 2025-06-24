using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ServiceImageDAO
    {
        private readonly WeddingWonderDbContext context;

        public ServiceImageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<ServiceImage>> GetServiceImages()
        {
            try
            {
                return await context.ServiceImages
                    .Include(si => si.Service)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ServiceImage>> GetImagesByServiceId(int serviceId)
        {
            try
            {
                List<ServiceImage>? images = await context.ServiceImages
                   .Where(i => i.ServiceId == serviceId).ToListAsync();

                if (images == null) throw new Exception("This service has no image");

                return images;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ServiceImage?> GetFirstImageOfService(int serviceId)
        {
            try
            {
                ServiceImage? imageInDb = await context.ServiceImages
                    .Where(i => i.ServiceId == serviceId)
                    .FirstOrDefaultAsync();

                return imageInDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ServiceImage> GetServiceImageById(int imageId)
        {
            try
            {
                return await context.ServiceImages
                    .Include(si => si.Service)
                    .FirstOrDefaultAsync(si => si.ImageId == imageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateServiceImage(ServiceImage serviceImage)
        {
            try
            {
                await context.ServiceImages.AddAsync(serviceImage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateServiceImage(int id, ServiceImage serviceImage)
        {
            try
            {
                context.ServiceImages.Update(serviceImage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteServiceImage(int imageId)
        {
            try
            {
                ServiceImage? serviceImageToDelete = await context.ServiceImages.FindAsync(imageId);
                if (serviceImageToDelete != null)
                {
                    context.ServiceImages.Remove(serviceImageToDelete);
                }
                else
                {
                    throw new Exception("ServiceImage not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
