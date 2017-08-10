using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.Web.Infrastructure;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoPageService: IPageService
    {
        private List<Page> _defaultPages = new List<Page>();

        public DemoPageService()
        {
            FillDefaultData();
        }

        private void FillDefaultData()
        {
            try
            {
                var file = "pages.xml";
                var xs = new XmlSerializer(typeof(List<Page>));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultPages = (List<Page>)xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultPages.Add(new Page()
                {
                    PageId = 1,
                    Title = "Главная страница",
                    RelativeUrl = "home",
                    Content = "<p>Добро пожаловать на главную</p>",
                    Annotation = "Краткое описание",
                    CreationDate = new DateTime(1970, 1, 1),
                    Layout = "clientLayout",
                    Keywords = "страница, rocms, главная"
                });
            }
        }

        private const string PAGES_SESSION_KEY = "Pages";
        private void InitSessionDataIfEmpty(HttpContext ctx)
        {
            if (ctx.Session[PAGES_SESSION_KEY] == null)
            {
                ctx.Session[PAGES_SESSION_KEY] = _defaultPages.ToList();
            }
        }

        private List<Page> GetSessionPages(HttpContext ctx)
        {
            return (List<Page>)ctx.Session[PAGES_SESSION_KEY];
        }

        public int CreatePage(Page page)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var pages = GetSessionPages(HttpContext.Current);
            int id = pages.Max(x => x.PageId) + 1;
            page.PageId = id;
            pages.Add(page);
            return id;
        }


        private void FillCanonicalUrl(PageInfo page)
        {
            page.CannonicalUrl = GetPageCanonicalUrl(page.RelativeUrl);
        }

        public Page GetPage(string url)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var res =
                GetSessionPages(HttpContext.Current)
                    .FirstOrDefault(x => x.RelativeUrl.Equals(url, StringComparison.InvariantCultureIgnoreCase));
            FillCanonicalUrl(res);
            return res;
        }

        public Page GetPage(int id)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var res =
                GetSessionPages(HttpContext.Current)
                    .FirstOrDefault(x => x.PageId == id);
            FillCanonicalUrl(res);
            return res;
        }

        public void UpdatePage(Page page)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var pages = GetSessionPages(HttpContext.Current);
            if(!pages.Any(x => x.PageId == page.PageId))
            {
                throw new ArgumentException("PageId");
            }
            // старая страница удаляется, новая добавляется
            pages.RemoveAll(x => x.PageId == page.PageId);
            pages.Add(page);
        }

        public void DeletePage(int pageId)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var pages = GetSessionPages(HttpContext.Current);
            if (!pages.Any(x => x.PageId == pageId))
            {
                throw new ArgumentException("PageId");
            }
            pages.RemoveAll(x => x.PageId == pageId);
        }

        public IList<PageInfo> GetPagesInfo()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var pages = GetSessionPages(HttpContext.Current);
            var res = new List<PageInfo>();
            foreach (var page in pages)
            {
                res.Add(page);
            }
            return res;
        }

        public string GetPageCanonicalUrl(string relativeUrl)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            string prefix = GetCanonicalPageUrlPrefix(relativeUrl);
            return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
        }

        public IList<PageInfo> GetSitemapPagesInfo()
        {
            return GetPagesInfo().Where(x => !x.HideInSitemap).ToList();
        }

        public string GetNextAvailableRelativeUrl(string relativeUrl)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var pages = GetSessionPages(HttpContext.Current);
            var existingPage = pages.First(x => x.RelativeUrl.Equals(relativeUrl, StringComparison.InvariantCultureIgnoreCase));
            if (existingPage == null)
                return relativeUrl;
            bool exists;
            string url;
            int counter = 1;
            do
            {
                counter++;
                url = $"{relativeUrl}-{counter}";
                existingPage = pages.First(x => x.RelativeUrl.Equals(url, StringComparison.InvariantCultureIgnoreCase));
                exists = existingPage != null;
            } while (exists);
            return url;
        }

        private string GetCanonicalPageUrlPrefix(string relativeUrl)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var pages = GetSessionPages(HttpContext.Current);
            string res = String.Empty;
            bool hasParent = false;
            var page = pages.First(x => x.RelativeUrl.Equals(relativeUrl, StringComparison.InvariantCultureIgnoreCase));
            if (page.ParentPageId.HasValue)
            {
                var parent = pages.Single(x => x.PageId == page.ParentPageId.Value);
                res = parent.RelativeUrl;
                hasParent = parent.ParentPageId.HasValue;
            }
            if (String.IsNullOrEmpty(res) || !hasParent)
            {
                return res;
            }

            return $"{GetCanonicalPageUrlPrefix(res)}/{res}";
        }
    }
}
