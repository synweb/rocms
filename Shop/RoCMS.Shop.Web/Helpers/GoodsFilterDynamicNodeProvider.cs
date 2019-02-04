using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.Helpers
{
    public class GoodsFilterDynamicNodeProvider : DynamicNodeProviderBase
    {
        IShopService shopService = DependencyResolver.Current.GetService<IShopService>();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            IHeartService categoryService = DependencyResolver.Current.GetService<IHeartService>();
            

            var nodes = new List<DynamicNode>();
            try
            {
                var categories = categoryService.GetHearts(typeof(Category).FullName);

                foreach (var category in categories)
                {
                    var goodsFilter = new GoodsFilter()
                    {
                        CategoryIds = new List<List<int>>()
                        {
                            new List<int>()
                            {
                                category.HeartId
                            }
                        }
                    };
                    int total;
                    FilterCollections filters;

                    shopService.GetGoodsSet(goodsFilter, 1, Int32.MaxValue, out total, out filters);

                    if (total > 0)
                    {
                        foreach (var manufacturer in filters.Manufacturers)
                        {

                            goodsFilter.ManufacturerIds = new List<int>() { manufacturer.ID };

                            string key = $"filter_{category.HeartId}_{manufacturer.ID}";
                            string title = $"{category.Title} {manufacturer.Name}";
                            nodes.Add(GetNode(goodsFilter, key, title));


                            foreach (var spec in filters.SpecValues)
                            {
                                foreach (var specValue in spec.Value)
                                {
                                    int total2;
                                    FilterCollections filters2;

                                    goodsFilter.SpecIdValues = new Dictionary<int, string>() { { spec.Key.SpecId, specValue} };
                                    shopService.GetGoodsSet(goodsFilter, 1, Int32.MaxValue, out total2, out filters2);
                                    if (total2 > 0)
                                    {
                                        string key2 = $"filter_{category.HeartId}_{manufacturer.ID}_spec{spec.Key.SpecId}_{specValue}";
                                        string title2 = $"{category.Title} {manufacturer.Name} {spec.Key.Name} {specValue}";
                                        nodes.Add(GetNode(goodsFilter, key2, title2));
                                    }
                                }
                                
                            }
                        }

                        goodsFilter.ManufacturerIds = new List<int>();

                        foreach (var spec in filters.SpecValues)
                        {
                            foreach (var specValue in spec.Value)
                            {
                                int total3;
                                FilterCollections filters3;

                                goodsFilter.SpecIdValues = new Dictionary<int, string>() { { spec.Key.SpecId, specValue } };
                                shopService.GetGoodsSet(goodsFilter, 1, Int32.MaxValue, out total3, out filters3);
                                if (total3 > 0)
                                {
                                    string key3 = $"filter_{category.HeartId}_spec{spec.Key.SpecId}_{specValue}";
                                    string title3 = $"{category.Title} {spec.Key.Name} {specValue}";
                                    nodes.Add(GetNode(goodsFilter, key3, title3));
                                }
                            }

                        }


                    }
                }
            }
            catch (Exception e)
            {
                // если возникают косяки, не добавляем ничего в сайтмеп
            }

            return nodes;
        }

        private DynamicNode GetNode(GoodsFilter goodsFilter, string key, string title)
        {
            DynamicNode dynamicNode = new DynamicNode();
            // ключ должен быть уникальным для каждой ноды
            dynamicNode.Key = key;
            dynamicNode.RouteValues.Add("relativeUrl", shopService.GoodsFilterToUrl(goodsFilter));
            dynamicNode.Route = typeof(GoodsFilter).FullName;
            dynamicNode.Title = title;
            dynamicNode.Protocol = "*";

            dynamicNode.Attributes.Add("visibility", "MvcSiteMapProvider.Web.Mvc.XmlSiteMapResult");

            return dynamicNode;
        }
    }
}