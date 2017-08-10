using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Cassette.HtmlTemplates;
using Cassette.Views;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Base.ForWeb.Models
{
    public abstract class PageBase : WebViewPage
    {

        protected ISettingsService Settings
        {
            get
            {
                return DependencyResolver.Current.GetService<ISettingsService>();
            }
        }


        protected IHtmlString Template([PathReference] params string[] paths)
        {
            foreach(string path in paths)
            {
                Bundles.Reference<HtmlTemplateBundle>(path);
            }
            return MvcHtmlString.Empty;
        }

        protected IHtmlString Js([PathReference] params string[] uriStrings)
        {
            foreach(string uriString in uriStrings)
            {
                Bundles.Reference(uriString);
            }
            return MvcHtmlString.Empty;
        }

        protected IHtmlString JsInline(Func<dynamic, HelperResult> jsFragmentRenderer)
        {
            string html = jsFragmentRenderer(null).ToHtmlString();
            Bundles.AddInlineScript(html, "inline");

            return MvcHtmlString.Empty;
        }

        protected IHtmlString Css([PathReference] params string[] uriStrings)
        {
            foreach(string uriString in uriStrings)
            {
                Bundles.Reference(uriString);
            }
            return MvcHtmlString.Empty;
        }

        protected IHtmlString RenderScripts()
        {
            IHtmlString scripts = Bundles.RenderScripts();
            string result = scripts.ToHtmlString() + Bundles.RenderScripts("inline").ToHtmlString();
            return new HtmlString(result);
        }

        protected IHtmlString RenderTemplates()
        {
            IHtmlString result = Bundles.Helper.Render<HtmlTemplateBundle>(null);
            return result;
        }

        protected IHtmlString RenderStylesheets()
        {
            return Bundles.RenderStylesheets();
        }

        protected IHtmlString Block(Block block)
        {
            if (block == null)
            {
                return new HtmlString(String.Empty);
            }

            return Html.Raw(ContentRenderHelper.RenderContent(block.Content));
        }

        protected IHtmlString Ro(string data)
        {
            return Html.Raw(ContentRenderHelper.RenderContent(data));
        }
    }
}