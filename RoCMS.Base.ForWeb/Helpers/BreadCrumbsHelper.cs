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
        
        public static IList<BreadCrumb> ForCurrentHeart(string url)
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

                var heartService = DependencyResolver.Current.GetService<IHeartService>();
                var settingService = DependencyResolver.Current.GetService<ISettingsService>();
                var indexPage = settingService.GetHomepageUrl();
                string[] urlSegments = url.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
                IList<BreadCrumb> res = new List<BreadCrumb>();

                foreach (var urlSegment in urlSegments)
                {
                    if(indexPage.Equals(urlSegment))
                        continue;
                    var heart = heartService.GetHeart(urlSegment);
                    if (heart != null)
                    {
                        res.Add(new BreadCrumb()
                        {
                            IsLast = urlSegment == urlSegments.Last(),
                            Title = string.IsNullOrEmpty(heart.BreadcrumbsTitle) ? heart.Title : heart.BreadcrumbsTitle,
                            Url = $"/{heart.CanonicalUrl}"
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
