using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class PhotographerRepository : IPhotographerRepository
    {
        private readonly PhotographerDAO _dao;

        public PhotographerRepository(PhotographerDAO dao)
        {
            _dao = dao;
        }

        public async Task<bool> CreateAsync(Photographer photographer)
        {
            return await _dao.CreatePhotographer(photographer);
        }

        public async Task DeleteAsync(int id)
        {
            await _dao.DeletePhotographer(id);
        }

        public async Task<Photographer> GetAsyncById(int id)
        {
            return await _dao.GetPhotographerById(id);
        }

        public async Task<List<Photographer>> GetsAsync()
        {
            return await _dao.GetPhotographers();
        }

        public async Task UpdateAsync(int id, Photographer photographer)
        {
            await _dao.UpdatePhotographer(id, photographer);
        }
    }
}
