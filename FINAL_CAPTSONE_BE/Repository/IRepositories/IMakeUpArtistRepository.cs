using BusinessObject.Models;
using Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IMakeUpArtistRepository : IRepository<MakeUpArtist>
    {
        // Phương thức lấy danh sách MakeUpArtists theo ServiceId
        Task<List<MakeUpArtist>> GetMakeUpArtistsByServiceIdAsync(int serviceId);
    }
}
