using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Base.Helpers;
using RoCMS.Base.Infrastructure;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Web.App_Start;
using RoCMS.Shop.Web.Controllers;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Services;
using Shop.Web;

namespace RoCMS.Shop.Web
{
    public class ShopModuleInitializer: IModuleInitializer
    {
        //private IEnumerable<string> _shopControllerActionNames;
        private const string MODULE_DIR = "shop";

        public ShopModuleInitializer()
        {
            //_shopControllerActionNames = typeof(ShopController).GetMethods().Select(x => x.Name);

        }
        public void Init()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles, MODULE_DIR);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            
            InitBreadcrumbs();
            RegisterSearch();
        }

        private void RegisterSearch()
        {
            //TODO: не проверялось. Проверить!
            var searchService = DependencyResolver.Current.GetService<ISearchService>();
            searchService.RegisterRules(typeof(GoodsItem), new List<IndexingRule>()
            {
                x =>
                {
                    var item = (GoodsItem) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyName,
                        EntityName = x.GetType().FullName,
                        EntityId = item.HeartId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Name),
                        ImageId = item.MainImageId,
                        Title = item.Name,
                        Weight = 2,
                        Text = item.Description,
                        Url = item.CannonicalUrl
                    };
                },
                x =>
                {
                    var item = (GoodsItem) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyDescription,
                        EntityName = x.GetType().FullName,
                        EntityId = item.HeartId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Description),
                        ImageId = item.MainImageId,
                        Title = item.Name,
                        Weight = 1,
                        Text = item.Description,
                        Url = item.CannonicalUrl
                    };
                },
                x =>
                {
                    var item = (GoodsItem) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyHtmlDescription,
                        EntityName = x.GetType().FullName,
                        EntityId = item.HeartId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.HtmlDescription),
                        ImageId = item.MainImageId,
                        Title = item.Name,
                        Weight = 1,
                        Text = item.Description,
                        Url = item.CannonicalUrl
                    };
                }
            });
        }

        private void InitBreadcrumbs()
        {
            string SHOP_URL = RouteConfig.ShopUrl;

            var shopService = DependencyResolver.Current.GetService<IShopService>();
            var categoryService = DependencyResolver.Current.GetService<IShopCategoryService>();

            
            // для товаров и категорий
            BreadCrumbsHelper.RegisterPattern(new BreadCrumbPattern(
                "^/" + SHOP_URL + @"(/[a-zA-Z_0-9а-яА-Я-]+)+", url =>
                {




                    var shopUrl = url.Replace("/" + SHOP_URL, "");
                    var res = new List<BreadCrumb>();
                    var pageService = DependencyResolver.Current.GetService<IPageService>();
                    var shopTitle = pageService.GetPage(SHOP_URL).Title;
                    res.Add(new BreadCrumb()
                    {
                        IsLast = false,
                        Title = shopTitle,
                        Url = "/" + SHOP_URL
                    });




                    var elements = shopUrl.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    for (int index = 0; index < elements.Length; index++)
                    {
                        var element = elements[index];
                        if (index != elements.Length - 1) // не последний элемент - всегда категория
                        {
                            if (categoryService.CategoryExists(element))
                            {
                                var cat = categoryService.GetCategory(element);
                                res.Add(new BreadCrumb()
                                {
                                    IsLast = false,
                                    Title = cat.Name,
                                    Url = $"/{SHOP_URL}/" + string.Join("/",elements.Take(index+1))
                                });
                            }
                        }
                        else  // последний элемент - Товар или категория
                        {
                            if (shopService.GoodsExists(element))
                            {
                                var goodsItem = shopService.GetGoods(element, false);
                                res.Add(new BreadCrumb()
                                {
                                    IsLast = true,
                                    Title = goodsItem.Name,
                                    Url = url
                                });
                            }
                            else if (categoryService.CategoryExists(element))
                            {
                                var cat = categoryService.GetCategory(element);
                                res.Add(new BreadCrumb()
                                {
                                    IsLast = true,
                                    Title = cat.Name,
                                    Url = url
                                });
                            }

                        }
                    }
                    return res;
                }
                ));
        }
    }
}