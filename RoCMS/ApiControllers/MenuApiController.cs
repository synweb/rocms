using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.Menus)]
    public class MenuApiController : ApiController
    {
        private readonly IMenuService _menuService;

        public MenuApiController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public Menu Get(int id)
        {
            return _menuService.GetMenu(id);
        }

        [HttpPost]
        public void Update(Menu menu)
        {
            try
            {
                _menuService.UpdateMenu(menu);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        public int Create(Menu menu)
        {
            return _menuService.CreateMenu(menu);
        }

        [HttpPost]
        public void Delete(int id)
        {
            _menuService.DeleteMenu(id);
        }

    }
}
