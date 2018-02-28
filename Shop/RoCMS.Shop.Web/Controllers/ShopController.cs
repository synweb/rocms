using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider;
using RoCMS.Base.ForWeb.Helpers;
//using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;
using RoCMS.Shop.Web.Models.Filters;
using RoCMS.Web.Contract.Models.Search;
using BreadCrumbsHelper = RoCMS.Shop.Web.Helpers.BreadCrumbsHelper;

namespace RoCMS.Shop.Web.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly ISessionValueProviderService _sessionService;
        private readonly IShopService _shopService;

        private readonly ISearchService _searchService;
        private readonly IShopClientService _clientService;
        private readonly IShopActionService _shopActionService;
        private readonly IShopCategoryService _shopCategoryService;
        private readonly IShopManufacturerService _shopManufacturerService;

        private readonly IHeartService _heartService;

        private readonly IPrincipalResolver _principalResolver;

        public ShopController(ISessionValueProviderService sessionService, IShopService shopService,
            ISearchService searchService, IShopClientService clientService, IShopActionService shopActionService,
            IShopCategoryService shopCategoryService, IShopManufacturerService shopManufacturerService,
            IPrincipalResolver principalResolver, IHeartService heartService)
        {
            _sessionService = sessionService;
            _shopService = shopService;
            _searchService = searchService;
            _clientService = clientService;
            _shopActionService = shopActionService;
            _shopCategoryService = shopCategoryService;
            _shopManufacturerService = shopManufacturerService;
            _principalResolver = principalResolver;
            _heartService = heartService;
        }

        public ActionResult Categories()
        {
            return PartialView();
        }

        public ActionResult GoodsAwaitingDialog()
        {

            int? userId = _principalResolver.GetUserIdIfAuthenticated();

            if (userId.HasValue)
            {
                var client = _clientService.GetClientByUserId(userId.Value);
                if (client != null)
                {
                    ViewBag.Phone = client.Phone;
                    ViewBag.Email = client.Email;
                }
            }

            return PartialView("_GoodsAwaitingDialog");
        }

        private List<List<int>> ParseCategoryFilter(string catFilter)
        {
            List<List<int>> cats = new List<List<int>>();
            if (catFilter != null)
            {
                // вид:             1,2;3
                // расшифровка:     1&2|3
                var orGroups = catFilter.Split(';');
                // сначала разбиваем на группы по "или"
                foreach (var orGroup in orGroups)
                {
                    var intGroup = new List<int>();
                    // формируем группы по "и"
                    intGroup.AddRange(orGroup.Split(',').Select(int.Parse));
                    cats.Add(intGroup);
                }
            }
            return cats;
        }

        [PagingFilter]
        [GoodsFilter]
        public ActionResult AllGoods(int page = 1, int pgsize = 10, int? country = null, int? manufacturerId = null,
            int? packId = null, string specs = null, SortCriterion? sort = null, string catFilter = null)
        {
            int totalCount;
            FilterCollections filters;

            //TODO: ЭТО ЯВНО КОСЯК. Передается page, ожидается - startIndex !!! Перепроверить всё
            var goods = _shopService.GetGoodsSet(new GoodsFilter() {ClientMode = true, CategoryIds = ParseCategoryFilter(catFilter) }, page, pgsize, out totalCount,
                out filters, true);
            ViewBag.TotalCount = totalCount;
            return PartialView("_GoodsPage", goods);
        }

        [PagingFilter]
        [GoodsFilter]
        public ActionResult Category(int id, int? country, int? manufacturerId, int? packId, string specs, SortCriterion? sort)
        {
            bool exists = _shopCategoryService.CategoryExists(id);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }


            var cat = _shopCategoryService.GetCategory(id);

            var routeValues = ParamExtractor.ExtractParamsForSEF(Request);
            var args = new RouteValueDictionary(routeValues);
            args.Add("relativeUrl", cat.CanonicalUrl);
            args.Remove("id");
            return RedirectPermanent(Url.RouteUrl("CatalogSEF", args));
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "CategorySEF", DynamicNodeProvider = "RoCMS.Shop.Web.Helpers.CategoryDynamicNodeProvider, RoCMS.Shop.Web")]
        [PagingFilter]
        [GoodsFilter]
        public ActionResult CategorySEF(string relativeUrl, int? country, int? manufacturerId, int? packId, string specs, SortCriterion? sort)
        {

            string pageUrl = relativeUrl.Split('/').Last();
            //bool exists = _heartService.CheckIfUrlExists(pageUrl);
            //if (!exists)
            //{
            //    throw new HttpException(404, "Not found");
            //}

            var cat = _heartService.GetHeart(pageUrl);
            var requestPath = Request.Path.Substring(1);
            if (!cat.CanonicalUrl.Equals(requestPath, StringComparison.InvariantCultureIgnoreCase))
            {
                var routeValues = Request.RequestContext.RouteData.Values;
                //routeValues["relativeUrl"] = cat.CanonicalUrl;

                return RedirectPermanent(Url.RouteUrl(typeof(Category).FullName, routeValues));
            }



            int id = cat.HeartId;

            int page = ViewBag.Page;
            int pgsize = ViewBag.PageSize;

            var filter = new GoodsFilter()
            {
                CategoryIds = new[] { new [] {id} },
                ClientMode = true
            };
            var goods = GetGoodsPage(filter, sort, specs, packId, country, manufacturerId, page, pgsize);
            return PartialView("GoodsPage", goods);
        }

        private const SortCriterion DEFAULT_SORT = SortCriterion.Article;

        private IList<GoodsItem> GetGoodsPage(GoodsFilter filter, SortCriterion? sort, string specs, int? packId, int? country, int? manufacturerId, int page, int pgsize)
        {
            int totalCount;
            int startIndex = (page - 1) * pgsize + 1;

            filter.SortBy = sort.HasValue ? sort.Value : DEFAULT_SORT;
            ViewBag.Sort = filter.SortBy;

            var specIdValues = new Dictionary<int, string>();
            if (specs != null)
            {
                foreach (var specIdValue in specs.Split(','))
                {
                    var idVal = specIdValue.Split(':');
                    specIdValues.Add(int.Parse(idVal[0]), idVal[1]);
                }
            }
            filter.SpecIdValues = specIdValues;

            if (packId.HasValue)
            {
                filter.Packs = new[] { packId.Value };
            }

            if (country.HasValue)
            {
                filter.Countries = new[] { country.Value };
            }

            if (manufacturerId.HasValue)
            {
                filter.ManufacturerIds = new[] { manufacturerId.Value };
            }

            FilterCollections collections;
            IList<GoodsItem> goods = _shopService.GetGoodsSet(filter,
            startIndex, pgsize, out totalCount, out collections);
            ViewBag.TotalCount = totalCount;
            if (filter.CategoryIds.Count() == 1)
            {
                ViewBag.CategoryId = filter.CategoryIds.First();
            }
            if (filter.ActionIds.Count() == 1)
            {
                ViewBag.ActionId = filter.ActionIds.First();
            }
            ViewBag.Countries = collections.Countries;
            ViewBag.HasPacks = collections.Packs.Any();
            ViewBag.Packs = collections.Packs;
            ViewBag.Manufacturers = collections.Manufacturers;
            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.RequestedSpecIdValues = specIdValues;
            ViewBag.SpecValues = collections.SpecValues;
            return goods;
        }



        [PagingFilter]
        [GoodsFilter]
        public ActionResult Action(int id, string specs, int? country, int? manufacturerId, int? packId, SortCriterion? sort, int page = 1, int pgsize = 10)
        {
            bool exists = _shopActionService.ActionExists(id);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }
            ViewBag.BreadCrumbs = BreadCrumbsHelper.ForShopAction(id);
            var filter = new GoodsFilter()
            {
                ActionIds = new[] { id }
            };
            var goods = GetGoodsPage(filter, sort, specs, packId, country, manufacturerId, page, pgsize);
            return PartialView("GoodsPage", goods);
        }

        public ActionResult Manufacturer(int id)
        {
            Manufacturer man = _shopManufacturerService.GetManufacturer(id);
            return View(man);
        }

        public ActionResult BestSellers(int count)
        {
            IList<GoodsItem> goods = _shopService.GetBestSellers(count);
            return PartialView("_GoodsSetTiles", goods);
        }

        public ActionResult Goods(int id)
        {
            bool exists = _shopService.GoodsExists(id);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }
            var goods = _shopService.GetGoods(id);

            return RedirectToRoutePermanent(typeof(GoodsItem).FullName, new { relativeURL = goods.CanonicalUrl });
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "GoodsSEF", DynamicNodeProvider = "RoCMS.Shop.Web.Helpers.GoodsItemDynamicNodeProvider, RoCMS.Shop.Web")]
        public ActionResult GoodsSEF(string relativeUrl)
        {
            string pageUrl = relativeUrl.Split('/').Last();
            //bool exists = _shopService.GoodsExists(pageUrl);
            //if (!exists)
            //{
            //    throw new HttpException(404, "Not found");
            //}

            var goodsItem = _shopService.GetGoods(pageUrl);

            var requestPath = Request.Path.Substring(1);
            if (!goodsItem.CanonicalUrl.Equals(requestPath, StringComparison.InvariantCultureIgnoreCase))
            {
                var routeValues = Request.RequestContext.RouteData.Values;
                //routeValues["relativeUrl"] = goodsItem.CanonicalUrl;

                return RedirectPermanent(Url.RouteUrl(typeof(GoodsItem).FullName, routeValues));
            }

            return PartialView("Goods", (object)pageUrl);
        }

        public ActionResult Cart()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            return PartialView(cartId);
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult NewGoodsItems(int count)
        {
            IList<GoodsItem> goods = _shopService.GetNewGoodsItems(count);
            return PartialView("_GoodsSetTiles", goods);
        }

        public ActionResult RecommendedGoods(int count, IList<IdNamePair<int>> categoryids, int goodsItemId)
        {
            IList<GoodsItem> goods = _shopService.GetRecommendedGoods(count, categoryids.Select(x => x.ID).ToArray(), goodsItemId);
            return PartialView("_GoodsSet", goods);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Personal()
        {
            RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
            if (currentPrincipal == null) return new HttpUnauthorizedResult();

            var client = _clientService.GetClientByUserId(currentPrincipal.UserId);

            IEnumerable<Order> orders = _clientService.GetOrdersByUserId(currentPrincipal.UserId);
            ViewBag.Orders = orders;

            return View(client);

        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult OrderLike(int id)
        {
            var goodsItem = _shopService.GetGoods(id, false);
            //TODO: единичку - в конфиг
            var cats = _shopCategoryService.GetCategory(1).ChildrenCategories;
            var category = cats.First(x => goodsItem.Categories.Any(y => x.HeartId==y.ID));
            return View(category);
        }
    }
}