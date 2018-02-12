using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Data.Gateways;

namespace RoCMS.News.Services
{
    public class RssCrawlingService: NewsService, IRssCrawlingService
    {
        private readonly RssCrawlerGateway _rssCrawlerGateway = new RssCrawlerGateway();
        private readonly RssCrawlerFilterGateway _rssCrawlerFilterGateway = new RssCrawlerFilterGateway();

        public ICollection<RssCrawler> GetCrawlers()
        {
            var dataRes = _rssCrawlerGateway.Select();
            var res = Mapper.Map<ICollection<RssCrawler>>(dataRes);
            foreach (var rssCrawler in res)
            {
                Fill(rssCrawler);
            }
            return res;
        }

        public RssCrawler GetCrawler(int id)
        {
            var dataRes = _rssCrawlerGateway.SelectOne(id);
            var res = Mapper.Map<RssCrawler>(dataRes);
            Fill(res);
            return res;
        }

        public void UpdateCrawlers(ICollection<RssCrawler> crawlers)
        {
            var existingCrawlers = _rssCrawlerGateway.Select();
            var dataRecs = Mapper.Map<ICollection<Data.Models.RssCrawler>>(crawlers);
            bool Comparer(Data.Models.RssCrawler x, Data.Models.RssCrawler y)
            {
                return x.RssCrawlerId == y.RssCrawlerId;
            }
            CollectionMergeHelper.MergeNewAndOld(
                newItems: dataRecs,
                existingItems: existingCrawlers,
                comparer: Comparer,
                create: (x) =>
                {
                    int id = _rssCrawlerGateway.Insert(x);
                    var dataFilters = Mapper.Map<ICollection<Data.Models.RssCrawlerFilter>>(
                        crawlers.Single(y => x.RssCrawlerId==y.RssCrawlerId).Filters);
                    foreach (var filter in dataFilters)
                    {
                        _rssCrawlerFilterGateway.Insert(filter);
                    }
                },
                update: (x) =>
                {
                    _rssCrawlerGateway.Update(x);
                    var filters = _rssCrawlerFilterGateway.SelectByRssCrawler(x.RssCrawlerId);
                    var dataFilters = Mapper.Map<ICollection<Data.Models.RssCrawlerFilter>>(
                        crawlers.Single(y => x.RssCrawlerId == y.RssCrawlerId).Filters);
                    foreach (var filter in filters)
                    {
                        _rssCrawlerFilterGateway.Delete(filter.RssCrawlerFilterId);
                    }
                    foreach (var filter in dataFilters)
                    {
                        filter.RssCrawlerId = x.RssCrawlerId;
                        _rssCrawlerFilterGateway.Insert(filter);
                    }
                    //вместо добавления и удаления можно сделать ещё один мёрдж
                },
                delete: (x) =>
                {
                    _rssCrawlerGateway.Delete(x.RssCrawlerId);
                    // фильтры выпилятся сами
                    return true;
                }
            ); 


        }

        private void Fill(RssCrawler rssCrawler)
        {
            var dataFilters = _rssCrawlerFilterGateway.SelectByRssCrawler(rssCrawler.RssCrawlerId);
            var filters = Mapper.Map<ICollection<RssCrawlerFilter>>(dataFilters);
            rssCrawler.Filters = filters;
        }
    }
}
