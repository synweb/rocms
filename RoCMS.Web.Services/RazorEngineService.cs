using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using RoCMS.Web.Contract.Infrastructure;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class RazorEngineService : IRazorEngineService
    {
        //public string Render<T>(T model, RazorEmailTemplate templateName) where T: class
        //{
        //    if (model == null)
        //    {
        //        throw new NullReferenceException();
        //    }
        //    string template = Resources.Strings.ResourceManager.GetString(Enum.GetName(typeof(RazorEmailTemplate), templateName));
        //    string result = Razor.Parse(template, model);

        //    return result;
        //}
        
        public string RenderEmailMessage(string template, object model = null)
        {
            using (var writer = new StringWriter())
            {
                var controllerContext = new ControllerContext();
                controllerContext.RouteData.Values.Add("controller", "Message");

                string viewPath = String.Format("_{0}", template);

                HttpContext httpContext = null; //данная переменная получает значение только в случае, если HttpContext.Current == null

                var httpContextWrapper = new HttpContextWrapper(HttpContext.Current ??
                    (httpContext = new HttpContext(new SimpleWorkerRequest("", "", new StringWriter()))));


                controllerContext.RequestContext.HttpContext = httpContextWrapper;
                controllerContext.HttpContext = httpContextWrapper;

                RoViewEngine razorViewEngine = ViewEngines.Engines.OfType<RoViewEngine>().First();

                var viewData = new ViewDataDictionary();
                if (model != null)
                {
                    viewData.Model = model;
                }
                ViewEngineResult viewResult = null;
                //while (viewResult == null)
                //{

                //}
                viewResult = razorViewEngine.FindPartialView(controllerContext, viewPath, false);
                var viewContext = new ViewContext(controllerContext, viewResult.View,
                    viewData, new TempDataDictionary(), writer);

                if (httpContext != null)
                {
                    HttpContext.Current = httpContext;
                }

                viewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
    }
}
