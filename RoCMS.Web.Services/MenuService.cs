using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Services;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Services;
using Menu = RoCMS.Web.Contract.Models.Menu;
using MenuItem = RoCMS.Web.Contract.Models.MenuItem;

namespace RoCMS.Web.Services
{
    public class MenuService : BaseCoreService, IMenuService
    {
        private readonly ISettingsService _settings;
        private readonly MenuGateway _menuGateway = new MenuGateway();
        private readonly MenuItemGateway _menuItemGateway = new MenuItemGateway();

        public MenuService(ISettingsService settings)
        {
            _settings = settings;
        }


        #region IMenuService

        public Menu GetMenu(int menuId)
        {
            {
                var dataMenu = _menuGateway.SelectOne(menuId);
                var menu = Mapper.Map<Menu>(dataMenu);
                FillMenu(menu);

                return menu;
            }
        }

        public List<Menu> GetMenus()
        {
            {
                var dataMenus = _menuGateway.Select();
                var res = Mapper.Map<List<Menu>>(dataMenus);

                foreach (var menu in res)
                {
                    FillMenu(menu);
                }

                return res;
            }
        }

        private void FillMenu(Menu menu)
        {
            var flatDataItems = _menuItemGateway.Select(menu.MenuId);

            var rootItems = Mapper.Map<List<MenuItem>>(flatDataItems
                .Where(x => x.ParentMenuItemId == null)
                .OrderBy(x => x.SortOrder));

            FillMenuItems(rootItems, flatDataItems);
            menu.Items.AddRange(rootItems);
        }

        private void FillMenuItems(List<MenuItem> itemsToFill, ICollection<Data.Models.MenuItem> allDataItems)
        {
            foreach (var item in itemsToFill)
            {
                var childItems = allDataItems.Where(x => x.ParentMenuItemId == item.MenuItemId).OrderBy(x => x.SortOrder);
                item.Items = Mapper.Map<List<MenuItem>>(childItems);
                FillMenuItems(item.Items, allDataItems);
            }
        }

        Menu IMenuService.GetMainMenu()
        {
            var id = _settings.GetMainMenuId();
            return GetMenu(id);
        }

        public int CreateMenu(Menu menu)
        {

            var dataMenu = Mapper.Map<Data.Models.Menu>(menu);


            int menuId;
            using (var ts = new TransactionScope())
            {
                menuId = _menuGateway.Insert(dataMenu);
                CreateMenuItems(menu.Items, menuId);
                ts.Complete();
            }
            return menuId;

        }

        private void CreateMenuItems(List<MenuItem> items, int menuId, int? parentMenuItemId = null)
        {
            var dataItems = Mapper.Map<List<Data.Models.MenuItem>>(items);
            SetMenuItemsSortOrder(dataItems);

            foreach (var dataItem in dataItems)
            {

                dataItem.MenuId = menuId;
                dataItem.ParentMenuItemId = parentMenuItemId;
                int id = _menuItemGateway.Insert(dataItem);

                var item = items.ElementAt(dataItems.IndexOf(dataItem));
                if (item.Items.Any())
                {
                    CreateMenuItems(item.Items, menuId, id);
                }
            }
        }


        private void DeleteItems(int parentMenuItemId, ICollection<Data.Models.MenuItem> allOriginItems)
        {
            var items = allOriginItems.Where(x => x.ParentMenuItemId == parentMenuItemId);
            foreach (var item in items)
            {
                DeleteItems(item.MenuItemId, allOriginItems);
                _menuItemGateway.Delete(item.MenuItemId);
            }
        }

        private void UpdateItems(ICollection<MenuItem> items, int menuId, int? parentMenuItemId = null)
        {

            ICollection<Data.Models.MenuItem> allOriginItems = _menuItemGateway.Select(menuId);


            var originItems = allOriginItems.Where(x => x.ParentMenuItemId == parentMenuItemId);
            //удаление из базы тех, что больше нет в коллекции
            foreach (var origin in originItems)
            {
                if (!items.Any(x => x.MenuItemId == origin.MenuItemId))
                {
                    DeleteItems(origin.MenuItemId, allOriginItems);
                    _menuItemGateway.Delete(origin.MenuItemId);
                }
            }

            var dataItems = Mapper.Map<List<Data.Models.MenuItem>>(items);
            SetMenuItemsSortOrder(dataItems);

            foreach (var dataItem in dataItems)
            {

                dataItem.MenuId = menuId;
                dataItem.ParentMenuItemId = parentMenuItemId;

                var item = items.ElementAt(dataItems.IndexOf(dataItem));
                if (originItems.Any(x => x.MenuItemId == dataItem.MenuItemId))
                {
                    _menuItemGateway.Update(dataItem);
                    if (item.Items.Any())
                    {
                        UpdateItems(item.Items, menuId, item.MenuItemId);
                    }
                    else
                    {
                        DeleteItems(item.MenuItemId, allOriginItems);//на случай, если удалены
                    }
                }
                else
                {
                    int id = _menuItemGateway.Insert(dataItem);
                    if (item.Items.Any())
                    {
                        CreateMenuItems(item.Items, menuId, id);
                    }
                }




            }
        }


        public void UpdateMenu(Menu menu)
        {

            var dataMenu = Mapper.Map<Data.Models.Menu>(menu);

            using (var ts = new TransactionScope())
            {
                _menuGateway.Update(dataMenu);
                UpdateItems(menu.Items, menu.MenuId);
                ts.Complete();
            }

        }

        public void DeleteMenu(int menuId)
        {
            _menuGateway.Delete(menuId);
        }

        #endregion

        #region Methods


        private void SetMenuItemsSortOrder(IEnumerable<Data.Models.MenuItem> menuItems)
        {
            int i = 0;
            foreach (var menuItem in menuItems)
            {
                menuItem.SortOrder = i;
                i++;
            }
        }

        #endregion

        protected override int CacheExpirationInMinutes => 30;
    }
}
