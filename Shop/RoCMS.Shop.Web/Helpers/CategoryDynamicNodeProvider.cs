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
    public class CategoryDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            IHeartService categoryService = DependencyResolver.Current.GetService<IHeartService>();
            var nodes = new List<DynamicNode>();
            try
            {
                var categories = categoryService.GetHearts(typeof(Category).FullName);
                foreach (var category in categories)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "category_" + category.HeartId;
                    dynamicNode.RouteValues.Add("relativeUrl", category.RelativeUrl);
                    dynamicNode.Route = typeof(Category).FullName;
                    dynamicNode.Title = category.Title;
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