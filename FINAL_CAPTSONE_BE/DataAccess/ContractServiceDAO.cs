using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ContractServiceDAO
    {
        private readonly WeddingWonderDbContext context;
        public ContractServiceDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<ContractService>> GetContractServices()
        {
            try
            {
                List<ContractService> contractServices = await context.ContractServices
                    .ToListAsync();

                return contractServices;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from ContractServiceDAO(GetContractServices): " + ex.Message, ex);
            }
        }
        public async Task<List<ContractService>> GetContractServicesByContractId(int contractId)
        {
            try
            {
                List<ContractService> contractServices = await context.ContractServices
                    .Where(c => c.ContractId == contractId)
                    .ToListAsync();

                return contractServices;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from ContractServiceDAO(GetContractServicesByContractId): " + ex.Message, ex);
            }
        }
        public async Task<List<ContractService>> GetContractServicesByServiceId(int serviceId)
        {
            try
            {
                List<ContractService> contractServices = await context.ContractServices
                    .Where(c => c.ServiceId == serviceId)
                    .ToListAsync();

                return contractServices;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from ContractServiceDAO(GetContractServicesByServiceId): " + ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateContractService(ContractService contractService)
        {
            try
            {
                await context.ContractServices.AddAsync(contractService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from ContractServiceDAO(CreateContractService): " + ex.Message, ex);
            }
        }
        public async Task UpdateContractService(int id, ContractService contractService)
        {
            try
            {
                context.ContractServices.Update(contractService);
            }
            catch (Exception ex)
            {
                throw new Exception("Error from ContractServiceDAO(UpdateContractService): " + ex.Message, ex);
            }
        }
    }
}
