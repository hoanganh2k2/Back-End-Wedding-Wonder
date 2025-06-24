using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IServiceImageRepository : IRepository<ServiceImage>
    {
        Task<List<ServiceImage>> GetImagesByServiceId(int serviceId);
        Task<ServiceImage> GetFirstImageOfService(int serviceId);
    }
}
