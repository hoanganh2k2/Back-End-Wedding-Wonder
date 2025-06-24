using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class ContractServiceRepository : IContractServiceRepository
    {
        private readonly ContractServiceDAO _dao;
        public ContractServiceRepository(ContractServiceDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(ContractService obj) => _dao.CreateContractService(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<ContractService> GetAsyncById(int id) => throw new NotImplementedException();

        public Task<List<ContractService>> GetContractServicesByContractId(int id) => _dao.GetContractServicesByContractId(id);
        public Task<List<ContractService>> GetContractServicesByServiceId(int id) => _dao.GetContractServicesByServiceId(id);

        public Task<List<ContractService>> GetsAsync() => _dao.GetContractServices();

        public Task UpdateAsync(int id, ContractService obj) => _dao.UpdateContractService(id, obj);
    }
}
