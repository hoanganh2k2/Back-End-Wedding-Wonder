using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ContractDAO
    {
        private readonly WeddingWonderDbContext context;

        public ContractDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Contract>> GetContracts()
        {
            return await context.Contracts.ToListAsync();
        }

        public async Task<Contract> GetContractById(int contractId)
        {
            return await context.Contracts.FindAsync(contractId);
        }

        public async Task<bool> CreateContract(Contract contract)
        {
            await context.Contracts.AddAsync(contract);

            return true;
        }

        public async Task<bool> UpdateContract(int id, Contract contract)
        {
            context.Contracts.Update(contract);

            return true;
        }

        public async Task<bool> DeleteContract(int contractId)
        {
            var contract = await context.Contracts.FindAsync(contractId);
            if (contract != null)
            {
                context.Contracts.Remove(contract);

                return true;
            }
            return false;
        }
    }
}