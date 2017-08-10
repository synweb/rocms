using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.Helpers
{
    public class NewsDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            INewsItemService newsItemService = DependencyResolver.Current.GetService<INewsItemService>();
            var nodes = new List<DynamicNode>();
            try
            {
                int total;
                var news = newsItemService.GetNewsPage(new NewsFilter() { OnlyPosted = true }, 1, int.MaxValue, out total);
                foreach (var newsItem in news)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "news_" + newsItem.NewsId;
                    dynamicNode.RouteValues.Add("relativeUrl", newsItem.CanonicalUrl);
                    dynamicNode.Route = "BlogItem";
                    dynamicNode.Title = newsItem.Title;

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