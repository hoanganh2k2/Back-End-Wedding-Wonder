using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly MenuDAO _dao;

        public MenuRepository(MenuDAO dao)
        {
            _dao = dao;
        }

        // Create a new menu
        public Task<bool> CreateAsync(Menu menu) => _dao.CreateMenu(menu);

        // Delete a menu by ID
        public Task DeleteAsync(int id) => _dao.DeleteMenu(id);

        // Get a specific menu by ID
        public Task<Menu> GetAsyncById(int id) => _dao.GetMenuById(id);

        // Get all menus
        public Task<List<Menu>> GetsAsync() => _dao.GetMenus();

        // Update an existing menu
        public Task UpdateAsync(int id, Menu menu) => _dao.UpdateMenu(id, menu);

        public Task<List<Menu>> GetMenusByCateringId(int cateringId) => _dao.GetMenusByCateringId(cateringId);
    }
}
