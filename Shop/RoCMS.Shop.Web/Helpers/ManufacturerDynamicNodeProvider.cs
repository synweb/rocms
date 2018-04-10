using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.Helpers
{
    public class ManufacturerDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            IShopManufacturerService manufacturerService = DependencyResolver.Current.GetService<IShopManufacturerService>();
            var nodes = new List<DynamicNode>();
            try
            {
                var manufacturers = manufacturerService.GetManufacturers();
                foreach (var manufacturer in manufacturers)
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    // ключ должен быть уникальным для каждой ноды
                    dynamicNode.Key = "manufacturer_" + manufacturer.HeartId;
                    dynamicNode.RouteValues.Add("relativeUrl", manufacturer.RelativeUrl);
                    dynamicNode.Route = typeof(Manufacturer).FullName;
                    dynamicNode.Title = manufacturer.Title;
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