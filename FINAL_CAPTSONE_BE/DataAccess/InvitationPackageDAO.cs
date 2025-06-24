using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class InvitationPackageDAO
    {
        private readonly WeddingWonderDbContext context;
        public InvitationPackageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<InvitationPackage>> GetInvitationPackages()
        {
            try
            {
                List<InvitationPackage> invitationPackages = await context.InvitationPackages
                        .ToListAsync();

                return invitationPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<InvitationPackage>> GetInvitationPackagesByServiceId(int serviceId)
        {
            try
            {
                List<InvitationPackage> invitationPackages = await context.InvitationPackages
                    .Where(s => s.ServiceId == serviceId && s.Status == true)
                    .ToListAsync();

                return invitationPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<InvitationPackage> GetInvitationPackageById(int packageId)
        {
            try
            {
                InvitationPackage? invitation = await context.InvitationPackages.FindAsync(packageId);

                return invitation;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateInvitationPackage(InvitationPackage invitation)
        {
            try
            {
                invitation.Status = true;
                await context.InvitationPackages.AddAsync(invitation);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateInvitationPackage(int id, InvitationPackage invitation)
        {
            try
            {
                context.InvitationPackages.Update(invitation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteInvitationPackage(int invitationPackageId)
        {
            try
            {
                InvitationPackage? invitationToDelete = await context.InvitationPackages.FindAsync(invitationPackageId);
                if (invitationToDelete != null)
                {
                    invitationToDelete.Status = false;
                    context.InvitationPackages.Update(invitationToDelete);
                }
                else
                {
                    throw new Exception("Outfit not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
