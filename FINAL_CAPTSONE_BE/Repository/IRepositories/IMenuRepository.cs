using BusinessObject.Models;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<List<Menu>> GetMenusByCateringId(int cateringId);
    }
}
