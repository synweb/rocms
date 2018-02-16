using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.News.Contract.Models;

namespace RoCMS.News.Contract.Services
{
    public interface IRssCrawlingService
    {
         ICollection<RssCrawler> GetCrawlers();
        RssCrawler GetCrawler(int id);
        void UpdateCrawlers(ICollection<RssCrawler> crawlers);
        void StartCrawling();
    }
}
