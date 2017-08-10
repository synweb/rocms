﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Helpers
{
    public class PageDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            IPageService pageService = DependencyResolver.Current.GetService<IPageService>();
            var nodes = new List<DynamicNode>();
            try
            {
                var pages = pageService.GetSitemapPagesInfo();
                foreach (var page in pages)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "page_" + page.PageId;
                    dynamicNode.RouteValues.Add("relativeUrl", page.CannonicalUrl);
                    dynamicNode.Route = "PageSEF";
                    dynamicNode.Title = page.Title;

                    dynamicNode.Attributes.Add("visibility", "MvcSiteMapProvider.Web.Mvc.XmlSiteMapResult");

                    nodes.Add(dynamicNode);
                }
            }
            catch
            {
                // что-то пошло не так, но из-за этого не должен падать весь сайт
            }

            return nodes;
        }
    }
}