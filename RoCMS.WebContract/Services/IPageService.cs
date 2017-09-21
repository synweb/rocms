using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IPageService
    {
        int CreatePage(Page page);

        Page GetPage(string url);
        Page GetPage(int id);

        void UpdatePage(Page page);

        void DeletePage(int pageId);

        IList<Page> GetPages();
        
        IList<Page> GetSitemapPagesInfo();
        
    }
}
