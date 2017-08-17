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
    [AuthorizeResourcesApi(RoCmsResources.Menus)]
    public class MenuApiController : ApiController
    {
        private readonly IMenuService _menuService;
        private readonly ILogService _logService;

        public MenuApiController(IMenuService menuService, ILogService logService)
        {
            _menuService = menuService;
            _logService = logService;
        }

        // TODO: переписать методы на ResultModel

        [HttpGet]
        public Menu Get(int id)
        {
            try
            {
                return _menuService.GetMenu(id);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                throw;
            }
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
                _logService.LogError(e);
            }
        }

        [HttpPost]
        public int Create(Menu menu)
        {
            try
            {
                return _menuService.CreateMenu(menu);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                throw;
            }
        }

        [HttpPost]
        public void Delete(int id)
        {
            try
            {
                _menuService.DeleteMenu(id);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                throw;
            }
        }

    }
}
