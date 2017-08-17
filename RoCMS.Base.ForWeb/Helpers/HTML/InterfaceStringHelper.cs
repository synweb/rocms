using System.Web.Mvc;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Base.ForWeb.Helpers.HTML
{
    public static class InterfaceStringHelper
    {
        private static readonly IInterfaceStringService _interfaceStringService;
        static InterfaceStringHelper()
        {
            _interfaceStringService = DependencyResolver.Current.GetService<IInterfaceStringService>();
        }

        public static MvcHtmlString iStr(this HtmlHelper html, string key)
        {
            var strVal = _interfaceStringService.GetString(key).Value;
            return new MvcHtmlString(strVal);
        }
    }
}
