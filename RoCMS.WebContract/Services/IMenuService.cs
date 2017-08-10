using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IMenuService
    {
        Menu GetMainMenu();
        int CreateMenu(Menu menu);
        Menu GetMenu(int menuId);
        List<Menu> GetMenus();
        void UpdateMenu(Menu menu);
        void DeleteMenu(int menuId);
    }
}
