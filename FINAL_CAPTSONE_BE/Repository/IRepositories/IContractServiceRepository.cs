using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IContractServiceRepository : IRepository<ContractService>
    {
        Task<List<ContractService>> GetContractServicesByContractId(int id);
        Task<List<ContractService>> GetContractServicesByServiceId(int id);
    }
}
