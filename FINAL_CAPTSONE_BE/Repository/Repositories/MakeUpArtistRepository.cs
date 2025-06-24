using BusinessObject.Models;
using DataAccess;
using DataAccess.DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class MakeUpArtistRepository : IMakeUpArtistRepository
    {
        private readonly MakeUpArtistDAO _dao;

        public MakeUpArtistRepository(MakeUpArtistDAO dao)
        {
            _dao = dao;
        }

        public async Task<bool> CreateAsync(MakeUpArtist artist)
        {
            return await _dao.CreateMakeUpArtist(artist);
        }

        public async Task DeleteAsync(int id)
        {
            await _dao.DeleteMakeUpArtist(id);
        }

        public async Task<MakeUpArtist> GetAsyncById(int id)
        {
            return await _dao.GetMakeUpArtistById(id);
        }

        public async Task<List<MakeUpArtist>> GetsAsync()
        {
            return await _dao.GetMakeUpArtists();
        }

        public async Task UpdateAsync(int id, MakeUpArtist artist)
        {
            await _dao.UpdateMakeUpArtist(id, artist);
        }

        public async Task<List<MakeUpArtist>> GetMakeUpArtistsByServiceIdAsync(int serviceId)
        {
            return await _dao.GetMakeUpArtistsByServiceId(serviceId);
        }
    }
}
