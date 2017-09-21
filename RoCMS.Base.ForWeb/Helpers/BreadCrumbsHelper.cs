using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Base.ForWeb.Helpers
{
    public static class BreadCrumbsHelper
    {
        private static IList<BreadCrumbPattern> _patterns = new List<BreadCrumbPattern>();

        public static void RegisterPattern(BreadCrumbPattern pattern)
        {
            _patterns.Add(pattern);
        }

        public static List<BreadCrumb> ForSearch()
        {
            var res = new List<BreadCrumb>
            {
                new BreadCrumb()
                {
                    IsLast = true,
                    Title = "Поиск",
                    Url = null
                }
            };
            return res;
        }
        
        public static IList<BreadCrumb> ForCurrentPage(string url)
        {
            try
            {
                foreach (var breadCrumbPattern in _patterns)
                {
                    if (Regex.IsMatch(url, breadCrumbPattern.Regex))
                    {
                        return breadCrumbPattern.Handler(url);
                    }
                }

                var pageService = DependencyResolver.Current.GetService<IPageService>();
                var settingService = DependencyResolver.Current.GetService<ISettingsService>();
                var indexPage = settingService.GetHomepageUrl();
                string[] pageUrls = url.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
                IList<BreadCrumb> res = new List<BreadCrumb>();

                foreach (var pageUrl in pageUrls)
                {
                    if(indexPage.Equals(pageUrl))
                        continue;
                    var page = pageService.GetPage(pageUrl);
                    if (page != null)
                    {
                        res.Add(new BreadCrumb()
                        {
                            IsLast = pageUrl == pageUrls.Last(),
                            Title = string.IsNullOrEmpty(page.BreadcrumbsTitle) ? page.Title : page.BreadcrumbsTitle,
                            Url = $"/{page.CannonicalUrl}"
                        });
                    }
                }

                return res;
            }
            catch
            {
                return new List<BreadCrumb>();
            }
        } 
    }
}
