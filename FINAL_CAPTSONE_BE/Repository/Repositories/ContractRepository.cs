using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ContractDAO _dao;
        public ContractRepository(ContractDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(Contract obj) => _dao.CreateContract(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<Contract> GetAsyncById(int id) => _dao.GetContractById(id);

        public Task<List<Contract>> GetsAsync() => _dao.GetContracts();

        public Task UpdateAsync(int id, Contract obj) => _dao.UpdateContract(id, obj);

   
    }
}
