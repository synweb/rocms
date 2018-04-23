using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.Controllers
{
    [AuthorizeResources(ShopRoCmsResources.Shop)]
    public class ShopEditorController : Controller
    {

        [MvcSiteMapNode(Title = "Каталог", ParentKey = "AdminHome", Key = "Shop", Clickable = false, VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop"", ""iconClass"" : ""fa-shopping-cart""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop)]
        public ActionResult Index()
        {
            return new EmptyResult();
        }

        [MvcSiteMapNode(Order = 8, Title = "Клиенты", ParentKey = "Shop", Key = "Shop_Clients", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Clients""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_Clients)]
        public ActionResult Clients()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 11, Title = "Настройки", ParentKey = "Shop", Key = "Shop_Settings", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Settings""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_Settings)]
        public ActionResult Settings()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 1, Title = "Товары", ParentKey = "Shop", Key = "Shop_GoodsEditor", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS",  Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop""}")]
        public ActionResult GoodsEditor()
        {
            return View();
        }

        [AuthorizeResources(ShopRoCmsResources.Shop_MassPriceChange)]
        [MvcSiteMapNode(Order = 14, Title = "Пакетное изменение цен", ParentKey = "Shop", Key = "Shop_MassPriceEditor", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_MassPriceChange""}")]
        public ActionResult MassPriceEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 12, Title = "Пункты самовывоза", ParentKey = "Shop", Key = "Shop_PickUpPoints", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_PickUpPoints""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_PickUpPoints)]
        public ActionResult PickUpPoints()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 5, Title = "Категории", ParentKey = "Shop", Key = "Shop_CategoriesEditor", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop""}")]
        public ActionResult CategoriesEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 6, Title = "Производители", ParentKey = "Shop", Key = "Shop_ManufacturersEditor", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop""}")]
        public ActionResult ManufacturersEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 4, Title = "Акции и скидки", ParentKey = "Shop", Key = "Shop_Actions", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Actions""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_Actions)]
        public ActionResult ActionsEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 3, Title = "Характеристики", ParentKey = "Shop", Key = "Shop_SpecsEditor", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop""}")]
        public ActionResult SpecsEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 7, Title = "Упаковки", ParentKey = "Shop", Key = "Shop_Packs", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Packs""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_Packs)]
        public ActionResult PacksEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 2, Title = "Наборы", ParentKey = "Shop", Key = "Shop_CompatiblesEditor", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop""}")]
        public ActionResult CompatiblesEditor()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 10, Title = "Экспорт в Я.Маркет", ParentKey = "Shop", Key = "Shop_YmlExport", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_YmlExport""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_YmlExport)]
        public ActionResult YmlExport()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 0, Title = "Заказы", ParentKey = "Shop", Key = "Shop_Orders", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Orders""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_Orders)]
        public ActionResult Orders(int? id)
        {
            ViewBag.ClientId = id;
            return View(id);
        }

        [MvcSiteMapNode(Order = 9, Title = "Отзывы", ParentKey = "Shop", Key = "Shop_Reviews", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Reviews""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_Reviews)]
        public ActionResult Reviews()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 13, Title = "Постоянным клиентам", ParentKey = "Shop", Key = "Shop_RegularClients", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_RegularClients""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_RegularClients)]
        public ActionResult RegularClients()
        {
            return View();
        }

        [MvcSiteMapNode(Order = 14, Title = "Экспорт базы", ParentKey = "Shop", Key = "Shop_DbExport", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_DbExport""}")]
        [AuthorizeResources(ShopRoCmsResources.Shop_DbExport)]
        public ActionResult DbExport()
        {
            return View();
        }

        [AuthorizeResources(ShopRoCmsResources.Shop_Currencies)]
        [MvcSiteMapNode(Order = 15, Title = "Валюты", ParentKey = "Shop", Key = "Shop_Currencies", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""visibility"": ""AdminMenu"", ""cmsResourceRequired"": ""Shop_Currencies""}")]
        public ActionResult Currencies()
        {
            return View();
        }
    }
}
