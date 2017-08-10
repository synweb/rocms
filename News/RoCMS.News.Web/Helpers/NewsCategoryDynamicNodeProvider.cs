using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.Helpers
{
    public class NewsСategoryDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            INewsCategoryService newsCategoryService = DependencyResolver.Current.GetService<INewsCategoryService>();
            var nodes = new List<DynamicNode>();
            try
            {
                var cats = newsCategoryService.GetAllCategories();
                foreach (var cat in cats)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "news_cat_" + cat.CategoryId;
                    dynamicNode.RouteValues.Add("relativeUrl", cat.CanonicalUrl);
                    dynamicNode.Route = "NewsCategoryItem";
                    dynamicNode.Title = cat.Name;

                    dynamicNode.Attributes.Add("visibility", "MvcSiteMapProvider.Web.Mvc.XmlSiteMapResult");

                    nodes.Add(dynamicNode);

                }
            }
            catch
            {
                
            }

            return nodes;
        }
    }
}