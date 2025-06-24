using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class RoleDAO
    {
        private readonly WeddingWonderDbContext context;

        public RoleDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Role>> GetRoles()
        {
            try
            {
                return await context.Roles
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Role> GetRoleById(int roleId)
        {
            try
            {
                return await context.Roles
                    .FindAsync(roleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateRole(Role role)
        {
            try
            {
                await context.Roles.AddAsync(role);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateRole(int id, Role role)
        {
            try
            {
                context.Roles.Update(role);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteRole(int roleId)
        {
            try
            {
                Role? roleToDelete = await context.Roles.FindAsync(roleId);
                if (roleToDelete != null)
                {
                    context.Roles.Remove(roleToDelete);
                }
                else
                {
                    throw new Exception("Role not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
