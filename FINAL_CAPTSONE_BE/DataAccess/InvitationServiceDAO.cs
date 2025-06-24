using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class InvitationServiceDAO
    {
        private readonly WeddingWonderDbContext context;

        public InvitationServiceDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<InvitationService>> GetInvitations()
        {
            try
            {
                List<InvitationService> intationsServices = await context.InvitationServices
                    .Include(s => s.InvitationPackages)
                    .ToListAsync();

                return intationsServices;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<InvitationService> GetInvitationById(int serviceId)
        {
            try
            {
                InvitationService? invitation = await context.InvitationServices
                    .Include(s => s.InvitationPackages)
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

                return invitation ?? throw new Exception("Invitation not found.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateInvitation(InvitationService service)
        {
            try
            {
                await context.InvitationServices.AddAsync(service);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateInvitation(int id, InvitationService service)
        {
            try
            {
                context.InvitationServices.Update(service);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}