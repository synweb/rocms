using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.Helpers
{
    //public class UserNewsDynamicNodeProvider : DynamicNodeProviderBase
    //{
    //    public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
    //    {
    //        INewsItemService newsItemService = DependencyResolver.Current.GetService<INewsItemService>();
    //        var nodes = new List<DynamicNode>();
    //        try
    //        {
    //            IBlogService blogService = DependencyResolver.Current.GetService<IBlogService>();
    //            var news = newsItemService.GetAllNews();
    //            foreach (var newsItem in news.Where(x => x.BlogId.HasValue && x.BlogId != 1 && !x.Categories.Any()))
    //            {
    //                DynamicNode dynamicNode = new DynamicNode();
    //                // ключ должен быть уникальным для каждой ноды
    //                dynamicNode.Key = "news_" + newsItem.HeartId;

    //                var blog = blogService.GetBlog(newsItem.BlogId.Value);
    //                dynamicNode.RouteValues.Add("newsUrl", newsItem.RelativeUrl);
    //                dynamicNode.RouteValues.Add("blogUrl", blog.RelativeUrl);
    //                dynamicNode.Route = "UserBlogItem";
    //              dynamicNode.Protocol = "*";
    //                dynamicNode.Title = newsItem.Title;

    //                dynamicNode.Attributes.Add("visibility", "MvcSiteMapProvider.Web.Mvc.XmlSiteMapResult");

    //                nodes.Add(dynamicNode);

    //            }
    //        }
    //        catch (Exception e)
    //        {

    //        }

    //        return nodes;
    //    }
    //}
}