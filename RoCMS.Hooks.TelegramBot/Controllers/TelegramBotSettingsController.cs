using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Hooks.TelegramBot.Controllers
{
    [AuthorizeResources(RoCmsResources.AdminPanel)]
    public class TelegramBotSettingsController: Controller
    {
        private ISettingsService _settingsService;
        public TelegramBotSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [MvcSiteMapNode(Title = "Телеграм", ParentKey = "AdminHome", Key = "TelegramBot", Clickable = true, VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""AdminPanel"", ""iconClass"" : ""fa-telegram""}")]
        [AuthorizeResources(RoCmsResources.AdminPanel)]
        public ActionResult Settings()
        {
            return View();
        }
    }
}
