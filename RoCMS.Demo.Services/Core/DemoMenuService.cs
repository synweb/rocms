using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoMenuService: IMenuService
    {
        private readonly ISettingsService _settings;
        private List<Menu> _defaultMenus = new List<Menu>();

        public DemoMenuService(ISettingsService settings)
        {
            _settings = settings;
            FillDefaultData();
        }

        private void FillDefaultData()
        {
            var file = "menus.xml";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
            try
            {
                var xs = new XmlSerializer(typeof(List<Menu>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultMenus = (List<Menu>)xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultMenus.Add(new Menu() {
                    MenuId = 1,
                    Name = "Главное",
                    Items = new List<MenuItem>()
                    {
                        new MenuItem()
                        {
                            MenuItemId =1,
                            Name = "Главная",
                            PageUrl = "home"
                        },
                        new MenuItem()
                        {
                            MenuItemId = 2,
                            Name = "Контакты",
                            PageUrl = "contacts"
                        }
                    }
                });
                //var xs = new XmlSerializer(_defaultMenus.GetType());
                //using (FileStream fs = new FileStream(path, FileMode.Create))
                //{
                //    xs.Serialize(fs, _defaultMenus);
                //}
            }
        }

        private const string MENUS_SESSION_KEY = "Menus";
        private void InitSessionDataIfEmpty(HttpContext ctx)
        {
            if (ctx.Session[MENUS_SESSION_KEY] == null)
            {
                ctx.Session[MENUS_SESSION_KEY] = _defaultMenus.ToList();
            }
        }

        private List<Menu> GetSessionMenus(HttpContext ctx)
        {
            return (List<Menu>)ctx.Session[MENUS_SESSION_KEY];
        }

        public Menu GetMainMenu()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var id = _settings.GetMainMenuId();
            return GetMenu(id);
        }

        public int CreateMenu(Menu menu)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var menus = GetSessionMenus(HttpContext.Current);
            int id = menus.Max(x => x.MenuId) + 1;
            menu.MenuId = id;
            menus.Add(menu);
            return id;
        }

        public Menu GetMenu(int menuId)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var menus = GetSessionMenus(HttpContext.Current);
            return menus.FirstOrDefault(x => x.MenuId == menuId);
        }

        public List<Menu> GetMenus()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var menus = GetSessionMenus(HttpContext.Current);
            return menus;
        }

        public void UpdateMenu(Menu menu)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var menus = GetSessionMenus(HttpContext.Current);
            if (!menus.Any(x => x.MenuId == menu.MenuId))
            {
                throw new ArgumentException("MenuId");
            }
            // старое меню удаляется, новая добавляется
            menus.RemoveAll(x => x.MenuId == menu.MenuId);
            menus.Add(menu);
        }

        public void DeleteMenu(int menuId)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var menus = GetSessionMenus(HttpContext.Current);
            if (!menus.Any(x => x.MenuId == menuId))
            {
                throw new ArgumentException("MenuId");
            }
            menus.RemoveAll(x => x.MenuId == menuId);
        }
    }
}
