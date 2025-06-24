using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OutfitImageDAO
    {
        private readonly WeddingWonderDbContext context;

        public OutfitImageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<OutfitImage>> GetAllOutfitImages()
        {
            try
            {
                List<OutfitImage>? images = await context.OutfitImages
                   .ToListAsync();

                if (images == null) throw new Exception("There are no images");

                return images;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<OutfitImage>> GetImagesByOutfitId(int outfitId)
        {
            try
            {
                List<OutfitImage>? images = await context.OutfitImages
                   .Where(i => i.OutfitId == outfitId).ToListAsync();

                if (images == null) throw new Exception("This outfit has no image");

                return images;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<OutfitImage> GetFirstImageOfOutfit(int outfitId)
        {
            try
            {
                OutfitImage? imageInDb = await context.OutfitImages
                    .Where(i => i.OutfitId == outfitId)
                    .FirstOrDefaultAsync();

                return imageInDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<OutfitImage> GetImageById(int imageId)
        {
            try
            {
                OutfitImage? imageInDb = await context.OutfitImages
                    .FindAsync(imageId);

                if (imageInDb == null) throw new Exception("Image not found");

                return imageInDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateOutfitImage(OutfitImage outfitImage)
        {
            try
            {
                await context.OutfitImages.AddAsync(outfitImage);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateOutfitImage(int id, OutfitImage outfitImage)
        {
            try
            {
                OutfitImage? imageInDb = await context.OutfitImages.FindAsync(id);
                if (imageInDb == null) throw new Exception("Image not found");

                imageInDb.ImageText = outfitImage.ImageText;

                context.OutfitImages.Update(imageInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteOutfitImage(int imageId)
        {
            try
            {
                OutfitImage? imageInDb = await context.OutfitImages.FindAsync(imageId);
                if (imageInDb != null)
                {
                    context.OutfitImages.Remove(imageInDb);
                }
                else
                {
                    throw new Exception("Image not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
