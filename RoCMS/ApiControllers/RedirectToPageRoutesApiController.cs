using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.RedirectToPageRoutes)]
    public class RedirectToPageRoutesApiController : ApiController
    {
        [HttpGet]
        public ICollection<RedirectToPageRouter> GetRedirectToPageRouters()
        {
            var redirects = new List<RedirectToPageRouter>();
            RedirectToPageRoutesConfigurationSection redirectSection = (RedirectToPageRoutesConfigurationSection)ConfigurationManager.GetSection("redirectToPageRoutes");
            foreach (RedirectPageRoute rec in redirectSection.RedirectPageRoutes)
            {
                if (string.IsNullOrEmpty(rec.Key))
                    continue;
                redirects.Add(new RedirectToPageRouter(rec.Key, rec.Value));
            }
            return redirects;
        }
        
        [HttpPost]
        public ResultModel SaveRedirectToPageRouters(ICollection<RedirectToPageRouter> redirects)
        {
            try
            {
                if (redirects == null)
                    return new ResultModel(false, "Null collection");
                var webConfiguration = WebConfigurationManager.OpenWebConfiguration("~");
                RedirectToPageRoutesConfigurationSection redirectSection = (RedirectToPageRoutesConfigurationSection)webConfiguration.GetSection("redirectToPageRoutes");
                redirectSection.RedirectPageRoutes.Clear();
                foreach (var widget in redirects.OrderBy(x => x.Key))
                {
                    redirectSection.RedirectPageRoutes.Add(new RedirectPageRoute()
                    {
                        Key = widget.Key,
                        Value = widget.Value
                    });
                }
                webConfiguration.Save();
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}