using System.Web.Mvc;
using RoCMS.Base.Helpers;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace RoCMS.Base.ForWeb.Models.Filters
{
    public class PagingFilterAttribute : ActionFilterAttribute
    {


        #region Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int pageSize;
            int page;
            bool allPages = ParsingHelper.ParseObject<string>(filterContext.HttpContext.Request.QueryString, "pgsize") == "all";

            if (allPages)
            {
                page = 1;
                pageSize = int.MaxValue;
            }
            else
            {
                page = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "page") ?? 1;
                pageSize = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "pgsize") ?? 10;
            }
            filterContext.Controller.ViewBag.Page = page;
            filterContext.Controller.ViewBag.PageSize = pageSize;

            filterContext.ActionParameters["pageSize"] = (int)pageSize;
            filterContext.ActionParameters["pageNumber"] = page;

            base.OnActionExecuting(filterContext);
        }



        

        #endregion
    }
}