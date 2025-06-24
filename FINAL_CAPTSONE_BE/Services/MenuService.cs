using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MenuService
    {
        private readonly IMenuRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IMenuRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MenuDTO>> GetAllMenusAsync()
        {
            List<Menu> menus = await _repository.GetsAsync();
            return menus.ConvertAll(menu => new MenuDTO
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                Price = menu.Price,
                MenuContent = menu.MenuContent,
                MenuType = menu.MenuType,
                Status = menu.Status
            });
        }

        public async Task<MenuDTO?> GetMenuByIdAsync(int menuId)
        {
            Menu? menu = await _repository.GetAsyncById(menuId);
            return menu == null ? null : new MenuDTO
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                Price = menu.Price,
                MenuContent = menu.MenuContent,
                MenuType = menu.MenuType,
                Status = menu.Status
            };
        }

        public async Task<List<MenuDTO>> GetMenusByCateringIdAsync(int cateringId)
        {
            List<Menu> menus = await _repository.GetMenusByCateringId(cateringId);
            return menus.ConvertAll(menu => new MenuDTO
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                Price = menu.Price,
                MenuContent = menu.MenuContent,
                CateringId = menu.CateringId,
                MenuType = menu.MenuType,
                Status = menu.Status
            });
        }

        public async Task<bool> CreateMenuAsync(MenuDTO dto)
        {
            Menu menu = new()
            {
                MenuName = dto.MenuName,
                Price = dto.Price,
                MenuContent = dto.MenuContent,
                MenuType = dto.MenuType,
                Status = dto.Status
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.CreateAsync(menu);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateMenuAsync(int menuId, MenuDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                Menu? existingMenu = await _repository.GetAsyncById(menuId);
                if (existingMenu == null) return false;

                existingMenu.MenuName = dto.MenuName;
                existingMenu.Price = dto.Price;
                existingMenu.MenuContent = dto.MenuContent;
                existingMenu.MenuType = dto.MenuType;
                existingMenu.Status = dto.Status;

                await _repository.UpdateAsync(menuId, existingMenu);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteMenuAsync(int menuId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.DeleteAsync(menuId);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
