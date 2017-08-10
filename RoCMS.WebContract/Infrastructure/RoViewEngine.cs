using System.Web.Mvc;

namespace RoCMS.Web.Contract.Infrastructure
{
    public class RoViewEngine : RazorViewEngine
    {
        public RoViewEngine()
        {
            var viewLocations = new[]
            {

                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/bin/Views/{1}/{0}.cshtml",  
                "~/bin/Views/Shared/{0}.cshtml",  
                // etc
            };

            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
    }
}
