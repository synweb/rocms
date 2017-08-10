using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using MvcSiteMapProvider;
using RoCMS.Base;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Helpers
{
    public class RoCMSSiteMapNodesVisibilityProvider: SiteMapNodeVisibilityProviderBase
    {
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            // Is a visibility attribute specified?
            bool resourceCheckSuccessful = false;
            bool visibilityCheckSuccessful = false;

            if (!node.Attributes.ContainsKey("visibility"))
            {
                visibilityCheckSuccessful = true;
            }
            else
            {
                string visibility = node.Attributes["visibility"] as string;
                if (string.IsNullOrEmpty(visibility))
                {
                    visibilityCheckSuccessful = true;
                }
                else
                {
                    string resource = visibility.Trim();
                    if (sourceMetadata.ContainsKey("name") && (string) sourceMetadata["name"] == resource)
                    {
                        visibilityCheckSuccessful = true;
                    }
                    else if (sourceMetadata.ContainsKey("HtmlHelper") && (string)sourceMetadata["HtmlHelper"] == resource)
                    {
                        visibilityCheckSuccessful = true;
                    }
                }

            }
            if (!visibilityCheckSuccessful)
            {
                return false;
            }

            if (!node.Attributes.ContainsKey("cmsResourceRequired"))
            {
                resourceCheckSuccessful = true;
            }
            else
            {
                string visibility = node.Attributes["cmsResourceRequired"] as string;
                if (string.IsNullOrEmpty(visibility))
                {
                    resourceCheckSuccessful = true;
                }
                else
                {
                    string resource = visibility.Trim();

                    //process visibility


                    RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
                    if (currentPrincipal != null && currentPrincipal.Identity.IsAuthenticated)
                    {

                        if (currentPrincipal.IsAuthorizedForResource(resource))
                        {
                            resourceCheckSuccessful = true;
                        }

                    }
                }
            }

            return resourceCheckSuccessful && visibilityCheckSuccessful;

        }
    }
}
