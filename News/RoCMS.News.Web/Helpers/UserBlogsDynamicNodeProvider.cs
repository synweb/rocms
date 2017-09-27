using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.Helpers
{
    //public class UserBlogsDynamicNodeProvider : DynamicNodeProviderBase
    //{
    //    public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
    //    {
    //        IBlogService blogService = DependencyResolver.Current.GetService<IBlogService>();
    //        var nodes = new List<DynamicNode>();

    //        try
    //        {
    //            var blogs = blogService.GetBlogs().Where(x => x.BlogId != 1 && !String.IsNullOrEmpty(x.RelativeUrl));
    //            foreach (var blog in blogs)
    //            {
    //                DynamicNode dynamicNode = new DynamicNode();
    //                // ключ должен быть уникальным для каждой ноды
    //                dynamicNode.Key = "blog_" + blog.BlogId;

    //                dynamicNode.RouteValues.Add("blogUrl", blog.RelativeUrl);
    //                dynamicNode.Route = "UserBlog";

    //                dynamicNode.Title = blog.Title;

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