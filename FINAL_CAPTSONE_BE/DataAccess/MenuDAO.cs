using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MenuDAO
    {
        private readonly WeddingWonderDbContext context;

        public MenuDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        // Get all menus
        public async Task<List<Menu>> GetMenus()
        {
            try
            {
                return await context.Menus
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Get menu by ID
        public async Task<Menu> GetMenuById(int menuId)
        {
            try
            {
                return await context.Menus
                    .FindAsync(menuId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Get menus by catering ID
        public async Task<List<Menu>> GetMenusByCateringId(int cateringId)
        {
            try
            {
                return await context.Menus
                    .Where(menu => menu.CateringId == cateringId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Create a new menu
        public async Task<bool> CreateMenu(Menu menu)
        {
            try
            {
                await context.Menus.AddAsync(menu);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Update an existing menu
        public async Task UpdateMenu(int id, Menu menu)
        {
            try
            {
                var existingMenu = await context.Menus.FindAsync(id);
                if (existingMenu != null)
                {
                    existingMenu.MenuName = menu.MenuName;
                    existingMenu.Price = menu.Price;
                    existingMenu.MenuContent = menu.MenuContent;
                    existingMenu.CateringId = menu.CateringId;
                    existingMenu.MenuType = menu.MenuType;
                    existingMenu.Status = menu.Status;

                    context.Menus.Update(existingMenu);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Menu not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // Delete a menu
        public async Task DeleteMenu(int menuId)
        {
            try
            {
                Menu? menuToDelete = await context.Menus.FindAsync(menuId);
                if (menuToDelete != null)
                {
                    context.Menus.Remove(menuToDelete);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Menu not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
