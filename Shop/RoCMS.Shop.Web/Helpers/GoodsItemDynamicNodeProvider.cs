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
    public class GoodsItemDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            IHeartService categoryService = DependencyResolver.Current.GetService<IHeartService>();
            var nodes = new List<DynamicNode>();
            try
            {
                var goodsItems = categoryService.GetHearts(typeof(GoodsItem).FullName);
                foreach (var item in goodsItems)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "goodsItem_" + item.HeartId;
                    dynamicNode.RouteValues.Add("relativeUrl", item.RelativeUrl);
                    dynamicNode.Route = typeof(GoodsItem).FullName;
                    dynamicNode.Title = item.Title;
                    dynamicNode.Protocol = "*";

                    dynamicNode.Attributes.Add("visibility", "MvcSiteMapProvider.Web.Mvc.XmlSiteMapResult");

                    nodes.Add(dynamicNode);

                }
            }
            catch (Exception e)
            {
                // если возникают косяки, не добавляем ничего в сайтмеп
            }

            return nodes;
        }

        
    }
}