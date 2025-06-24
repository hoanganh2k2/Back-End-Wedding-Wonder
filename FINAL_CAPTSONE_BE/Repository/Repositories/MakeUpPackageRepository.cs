using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class MakeUpPackageRepository : IMakeUpPackageRepository
    {
        private readonly MakeUpPackageDAO _dao;

        public MakeUpPackageRepository(MakeUpPackageDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(MakeUpPackage obj) => _dao.CreateMakeUpPackage(obj);

        public Task DeleteAsync(int id) => _dao.DeleteMakeUpPackage(id);

        public Task<MakeUpPackage> GetAsyncById(int id) => _dao.GetMakeUpPackageById(id);

        public Task<List<MakeUpPackage>> GetAsyncByServiceId(int serviceid) => _dao.GetMakeUpPackageByServiceId(serviceid);

        public Task<List<MakeUpPackage>> GetsAsync() => _dao.GetMakeUpPackages();

        public Task UpdateAsync(int id, MakeUpPackage obj) => _dao.UpdateMakeUpPackage(id, obj);

        public Task<List<MakeUpPackage>> GetAsyncByEventType(int eventType) => _dao.GetMakeUpPackagesByEventTypeForCombo(eventType);
    }
}