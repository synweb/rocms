﻿using System;
using System.Collections.Generic;
using System.Configuration;
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
using RoCMS.Shop.Contract.Models.Exceptions;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;
using RoCMS.Shop.Web.Models.Filters;
using RoCMS.Web.Contract.Models.Search;
using Action = RoCMS.Shop.Contract.Models.Action;
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
        private readonly IShopOrderService _shopOrderService;

        private readonly IShopPickupPointService _pickupPointService;

        private readonly IHeartService _heartService;

        private readonly IPrincipalResolver _principalResolver;

        private readonly string _shopUrl;

        public ShopController(ISessionValueProviderService sessionService, IShopService shopService,
            ISearchService searchService, IShopClientService clientService, IShopActionService shopActionService,
            IShopCategoryService shopCategoryService, IShopManufacturerService shopManufacturerService,
            IPrincipalResolver principalResolver, IHeartService heartService, IShopOrderService shopOrderService, IShopPickupPointService pickupPointService)
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
            _shopOrderService = shopOrderService;
            _pickupPointService = pickupPointService;


            var service = DependencyResolver.Current.GetService<IShopSettingsService>();
            try
            {
                var settings = service.GetShopSettings();
                _shopUrl = String.IsNullOrEmpty(settings.ShopUrl) ? "catalog" : settings.ShopUrl;
            }
            catch
            {
                _shopUrl = "catalog";
            }
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

        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult AllGoods(int pageNumber, int pageSize, object specs, int? country = null, int? manufacturerId = null,
            int? packId = null, SortCriterion? sort = null, List<List<int>> catFilter = null, int? minPrice = null, int? maxPrice = null, string query = null)
        {
            //int totalCount;
            //FilterCollections filters;

            //Не дает, так как child action
            //var routeValues = Request.RequestContext.RouteData.Values;
            //if (catFilter != null && (catFilter.Count() == 1 && catFilter.First().Count() == 1))
            //{
            //    var heart = _heartService.GetHeart(catFilter.First().First());

            //    routeValues["relativeUrl"] = heart.RelativeUrl;
            //    return RedirectPermanent(Url.RouteUrl(typeof(Category).FullName, routeValues));
            //}


            int startIndex = (pageNumber - 1) * pageSize + 1;


            var goods = GetGoodsPage(new GoodsFilter()
            {
                ClientMode = true,
                CategoryIds = catFilter,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SearchPattern = query
            }, sort, (Dictionary<int, string>)specs, packId, country, manufacturerId, pageNumber, pageSize);

            return PartialView("_GoodsPage", goods);
        }

        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult Category(int id, int? country, int? manufacturerId, int? packId, object specs, SortCriterion? sort, List<List<int>> catFilter = null, int? minPrice = null, int? maxPrice = null, string query = null)
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
            return RedirectPermanent(Url.RouteUrl(typeof(Category).FullName, args));
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "CategorySEF", DynamicNodeProvider = "RoCMS.Shop.Web.Helpers.CategoryDynamicNodeProvider, RoCMS.Shop.Web")]
        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult CategorySEF(string relativeUrl, int? country, int? manufacturerId, int? packId, object specs, SortCriterion? sort, 
            List<List<int>> catFilter = null, int? minPrice = null, int? maxPrice = null, string query = null)
        {

            string pageUrl = relativeUrl.Split('/').Last();
            bool exists = _heartService.CheckIfUrlExists(pageUrl);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }
            var routeValues = Request.RequestContext.RouteData.Values;
            if (catFilter != null && (catFilter.Count() > 1 || catFilter.Any(x => x.Count() > 1)))
            {
                routeValues["relativeUrl"] = _shopUrl;
                return RedirectPermanent(Url.RouteUrl(typeof(Page).FullName, routeValues));
            }


            var cat = _heartService.GetHeart(pageUrl);

            if (catFilter != null && catFilter.Count() == 1 && catFilter.First().Count() == 1)
            {
                int catFilterId = catFilter.First().First();
                if (cat.HeartId != catFilterId)
                {
                    var filterCat = _heartService.GetHeart(catFilterId);
                    routeValues["relativeUrl"] = filterCat.RelativeUrl;
                    routeValues.Remove("cats");
                    return RedirectPermanent(Url.RouteUrl(typeof(Category).FullName, routeValues));
                }
            }


            var requestPath = Request.Path.Substring(1);
            if (!cat.CanonicalUrl.Equals(requestPath, StringComparison.InvariantCultureIgnoreCase))
            {

                //routeValues["relativeUrl"] = cat.CanonicalUrl;

                return RedirectPermanent(Url.RouteUrl(typeof(Category).FullName, routeValues));
            }

            int id = cat.HeartId;

            int page = ViewBag.Page;
            int pgsize = ViewBag.PageSize;

            var filter = new GoodsFilter()
            {
                CategoryIds = new[] { new[] { id } },
                ClientMode = true,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SearchPattern = query
            };
            var goods = GetGoodsPage(filter, sort, (Dictionary<int, string>) specs, packId, country, manufacturerId, page, pgsize);

            ViewBag.PagingRoute = typeof(Category).FullName;
            ViewBag.CategoryId = id;

            TempData["MetaKeywords"] = cat.MetaKeywords;
            TempData["MetaDescription"] = cat.MetaDescription;
            TempData["AdditionalHeaders"] = cat.AdditionalHeaders;

            return PartialView("GoodsPage", goods);
        }

        private const SortCriterion DEFAULT_SORT = SortCriterion.Article;

        private IList<GoodsItem> GetGoodsPage(GoodsFilter filter, SortCriterion? sort, object specs, int? packId, int? country, int? manufacturerId, int pageNumber, int pageSize)
        {
            int totalCount;
            int startIndex = (pageNumber - 1) * pageSize + 1;

            filter.SortBy = sort.HasValue ? sort.Value : DEFAULT_SORT;
            ViewBag.Sort = filter.SortBy;


            var specIdValues = specs;
            filter.SpecIdValues = (Dictionary<int, string>)specs;

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


            IList<GoodsItem> goods = _shopService.GetGoodsSet(filter,
            startIndex, pageSize, out totalCount, out var requestCollections);
            ViewBag.TotalCount = totalCount;
            if (filter.CategoryIds.Count() == 1 && filter.CategoryIds.First().Count() == 1)
            {
                ViewBag.CategoryId = filter.CategoryIds.First().First();
            }
            if (filter.ActionIds.Count() == 1)
            {
                ViewBag.ActionId = filter.ActionIds.First();
            }

            //TODO: для простоты пока что берем для всех категорий
            var categoryFilter = new GoodsFilter()
            {
                CategoryIds = filter.CategoryIds,
                ClientMode = filter.ClientMode,
                SearchPattern = filter.SearchPattern,
            };

            IList<GoodsItem> catgoods = _shopService.GetGoodsSet(categoryFilter,
                1, 1, out int totalCount2, out var collections);

            ViewBag.Countries = collections.Countries;
            ViewBag.HasPacks = collections.Packs.Any();
            ViewBag.Packs = collections.Packs;
            ViewBag.Manufacturers = collections.Manufacturers;
            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.RequestedSpecIdValues = specIdValues;
            ViewBag.SpecValues = collections.SpecValues;



            return goods;
        }



        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult Action(int id, object specs, int? country, int? manufacturerId, int? packId, SortCriterion? sort, int pageNumber, int pageSize, int? minPrice = null, int? maxPrice = null, string query = null)
        {
            bool exists = _shopActionService.ActionExists(id);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }
            ViewBag.BreadCrumbs = BreadCrumbsHelper.ForShopAction(id);
            var filter = new GoodsFilter()
            {
                ActionIds = new[] { id },
                SearchPattern = query
            };
            var goods = GetGoodsPage(filter, sort, (Dictionary<int, string>) specs, packId, country, manufacturerId, pageNumber, pageSize);
            return PartialView("GoodsPage", goods);
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "ActionSEF", DynamicNodeProvider = "RoCMS.Shop.Web.Helpers.ActionDynamicNodeProvider, RoCMS.Shop.Web")]
        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult ActionSEF(string relativeUrl, object specs, int? country, int? manufacturerId, int? packId, SortCriterion? sort, int pageNumber, int pageSize, int? minPrice = null, int? maxPrice = null, string query = null)
        {
            string pageUrl = relativeUrl.Split('/').Last();
            bool exists = _heartService.CheckIfUrlExists(pageUrl);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }

            var heart = _heartService.GetHeart(pageUrl);

            var filter = new GoodsFilter()
            {
                ActionIds = new[] { heart.HeartId },
                SearchPattern = query
            };

            ViewBag.PagingRoute = typeof(Action).FullName;
            

            var goods = GetGoodsPage(filter, sort, (Dictionary<int, string>)specs, packId, country, manufacturerId, pageNumber, pageSize);

            ViewBag.ActionId = heart.HeartId;

            TempData["MetaKeywords"] = heart.MetaKeywords;
            TempData["MetaDescription"] = heart.MetaDescription;
            TempData["AdditionalHeaders"] = heart.AdditionalHeaders;

            return PartialView("GoodsPage", goods);
        }

        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult Manufacturer(int id)
        {
            Manufacturer man = _shopManufacturerService.GetManufacturer(id);
            return View(man);
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "ManufacturerSEF", DynamicNodeProvider = "RoCMS.Shop.Web.Helpers.ManufacturerDynamicNodeProvider, RoCMS.Shop.Web")]
        [ShopPagingFilter]
        [GoodsFilter]
        public ActionResult ManufacturerSEF(string relativeUrl)
        {
            string pageUrl = relativeUrl.Split('/').Last();
            bool exists = _heartService.CheckIfUrlExists(pageUrl);
            if (!exists)
            {
                throw new HttpException(404, "Not found");
            }

            var heart = _heartService.GetHeart(pageUrl);

            Manufacturer man = _shopManufacturerService.GetManufacturer(heart.HeartId);

            TempData["MetaKeywords"] = heart.MetaKeywords;
            TempData["MetaDescription"] = heart.MetaDescription;
            TempData["AdditionalHeaders"] = heart.AdditionalHeaders;


            return View("Manufacturer", man);
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
            try
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

                TempData["MetaKeywords"] = goodsItem.MetaKeywords;
                TempData["MetaDescription"] = goodsItem.MetaDescription;
                TempData["AdditionalHeaders"] = goodsItem.AdditionalHeaders;

                return PartialView("Goods", (object) pageUrl);
            }
            catch (GoodsNotFoundException)
            {
                throw new HttpException(404, "Not found");
            }
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

            //IEnumerable<Order> orders = _clientService.GetOrdersByUserId(currentPrincipal.UserId);
            if (client != null)
            {
                int total;
                IEnumerable<Order> orders =
                    _shopOrderService.GetOrderPage(1, Int32.MaxValue, out total, client.ClientId);
                ViewBag.Orders = orders;
            }

            return View(client);

        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult PayForOrder(int id)
        {
            var order = _shopOrderService.GetOrder(id);

            ViewBag.ShopId = ConfigurationManager.AppSettings["YaKassaShopId"];
            ViewBag.ScId = ConfigurationManager.AppSettings["YaKassaScId"];

            return View(order);
        }

        public ActionResult PaymentFailed(int orderNumber)
        {
            var order = _shopOrderService.GetOrder(orderNumber);
            return View(order);
        }

        public ActionResult OrderLike(int id)
        {
            var goodsItem = _shopService.GetGoods(id, false);
            //TODO: единичку - в конфиг
            var cats = _shopCategoryService.GetCategory(1).ChildrenCategories;
            var category = cats.First(x => goodsItem.Categories.Any(y => x.HeartId == y.ID));
            return View(category);
        }

        public ActionResult PickUpPoint(int id)
        {
            try
            {
                PickupPointInfo pickUpPoint = _pickupPointService.GetPickupPoint(id);
                return View(pickUpPoint);
            }
            catch
            {
                throw new HttpException(404, "Not found");
            }
        }
    }
}