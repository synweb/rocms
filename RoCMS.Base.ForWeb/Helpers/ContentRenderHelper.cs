using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Web.Contract.Infrastructure;

namespace RoCMS.Base.ForWeb.Helpers
{
    public static class ContentRenderHelper
    {
        static ContentRenderHelper()
        {
            var configSection = ConfigurationManager.GetSection("pageRenderHelper") as PageRenderHelperConfigurationSection;
            if (configSection == null)
            {
                return;
            }

            foreach (EmbeddableView view in configSection.EmbeddableViews)
            {
                ViewsDictionary.Add(view.Pattern, view.Path);
            }
        }

        private static readonly Dictionary<string, Type> ParseRules = new Dictionary<string, Type>
        {
            // число 1234567890
            {@"\d+",typeof(int)},
            // дата 01.01.1970
            {@"\d{1,2}\.\d{1,2}\.\d{4}",typeof(string)},
            // строка STRING
            {@"[A-Z]+",typeof(string)},
            // строка ST_ri-nG
            {@"[a-zA-Z0-9-_]+",typeof(string)},
            {@"[а-яА-Я -]+", typeof(string)}

        };

        private static readonly Dictionary<string, string> ViewsDictionary = new Dictionary<string, string>();

        public static string RenderContent(string content)
        {
            if (String.IsNullOrWhiteSpace(content))
            {
                return String.Empty;
            }

            foreach (KeyValuePair<string, string> pair in ViewsDictionary)
            {
                string expr = ($@"\[\[{pair.Key.Replace("(", @"\(").Replace(")", @"\)")}\]\]");

                while (Regex.IsMatch(content, expr))
                {
                    // Выражение содержит параметр в круглых скобках
                    bool parametrized = false;
                    foreach (var rule in ParseRules)
                    {
                        if (expr.Contains(string.Format(@"\({0}\)", rule.Key)))
                        {
                            parametrized = true;
                            content = Replace(content, expr, rule.Key, rule.Value, pair.Value);
                            break;
                        }
                        else if (expr.Contains(@"\(") && expr.Contains(@"\)"))
                        {
                            var customRule = Regex.Match(expr, @"(?<=\\\()(.+?)(?=\\\))").Value;
                            parametrized = true;
                            content = Replace(content, expr, customRule, typeof(string), pair.Value);
                            break;
                        }
                    }

                    if (!parametrized)
                    {
                        content = Regex.Replace(content, expr, RenderPartialRazorView(pair.Value));
                    }
                }
            }
            return content;
        }

        private static string Replace(string content, string expr, string rule, Type paramType, string viewPath)
        {
            // Поиск выражения в тексте
            var contentExpr = Regex.Match(content, expr);

            var embracedParamRule = String.Format(@"(?<=\(){0}(?=\))", rule);

            string param = Regex.Match(contentExpr.Value, embracedParamRule).Value;
            var typedParam = Convert.ChangeType(param, paramType);
            //var paramExpr = expr.Replace(rule, param.ToString(CultureInfo.InvariantCulture));
            //var paramExpr = Regex.Replace(rule, replaceExpr, param.ToString(CultureInfo.InvariantCulture));
            string res = content.Replace(contentExpr.Value, RenderPartialRazorView(viewPath, typedParam));
            //Regex.Replace(content, contentExpr.Value, RenderPartialRazorView(viewPath, typedParam));
            return res;
        }

        static string RenderPartialRazorView(string viewPath, object model = null)
        {
            using (var writer = new StringWriter())
            {
                var controllerContext = new ControllerContext();

                // Выборка директории, в которой находится вьюха
                var viewDir = Regex.Match(viewPath, @"(?<=~(/bin)?/Views/)\w+(?=/.*)").Value;
                if (viewDir == "Shared")
                {
                    controllerContext.RouteData.Values.Add("controller", "Home");
                }
                else
                {
                    controllerContext.RouteData.Values.Add("controller", viewDir);
                }

                var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
                controllerContext.RequestContext.HttpContext = httpContextWrapper;
                controllerContext.HttpContext = httpContextWrapper;

                RoViewEngine razorViewEngine = ViewEngines.Engines.OfType<RoViewEngine>().First();

                var viewData = new ViewDataDictionary();
                if (model != null)
                {
                    viewData.Model = model;
                }


                ViewEngineResult viewResult = razorViewEngine.FindPartialView(controllerContext, viewPath, false);
                var viewContext = new ViewContext(controllerContext, viewResult.View,
                                                  viewData, new TempDataDictionary(), writer);

                viewResult.View.Render(viewContext, writer);

                return writer.ToString();
            }
        }
    }
}
