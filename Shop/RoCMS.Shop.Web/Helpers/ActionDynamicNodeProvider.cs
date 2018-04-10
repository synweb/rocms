using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using Action = RoCMS.Shop.Contract.Models.Action;

namespace RoCMS.Shop.Web.Helpers
{
    public class ActionDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            IShopActionService actionService = DependencyResolver.Current.GetService<IShopActionService>();
            var nodes = new List<DynamicNode>();
            try
            {
                var actions = actionService.GetActions();
                foreach (var action in actions)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "action_" + action.HeartId;
                    dynamicNode.RouteValues.Add("relativeUrl", action.RelativeUrl);
                    dynamicNode.Route = typeof(Action).FullName;
                    dynamicNode.Title = action.Title;
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