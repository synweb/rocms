using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Services;
using RoCMS.News.Contract.Models;
using Category = RoCMS.News.Data.Models.Category;
using NewsItem = RoCMS.News.Data.Models.NewsItem;
using TagStat = RoCMS.News.Data.Models.TagStat;

namespace RoCMS.News.Services
{
    public abstract class NewsService : BaseCacheService
    {
        static NewsService()
        {
            ConfigureMapper();
        }

        protected override int CacheExpirationInMinutes => AppSettingsHelper.HoursToExpireCartCache * 60;

        private static void ConfigureMapper()
        {
            Mapper.CreateMap<Blog, Data.Models.Blog>();
            Mapper.CreateMap<Data.Models.Blog, Blog>();

            Mapper.CreateMap<RecordType, Data.Models.RecordType>();
            Mapper.CreateMap<Data.Models.RecordType, RecordType>();
            Mapper.CreateMap<NewsItem, Contract.Models.NewsItem>()
                .ForMember(x => x.CreationDate, x => x.Ignore())
                .ForMember(x => x.RelativeUrl, x => x.Ignore())
                .ForMember(x => x.ParentHeartId, x => x.Ignore())
                .ForMember(x => x.BreadcrumbsTitle, x => x.Ignore())
                .ForMember(x => x.Noindex, x => x.Ignore())
                .ForMember(x => x.Title, x => x.Ignore())
                .ForMember(x => x.MetaDescription, x => x.Ignore())
                .ForMember(x => x.MetaKeywords, x => x.Ignore())
                .ForMember(x => x.Styles, x => x.Ignore())
                .ForMember(x => x.Scripts, x => x.Ignore())
                .ForMember(x => x.Layout, x => x.Ignore())
                .ForMember(x => x.AdditionalHeaders, x => x.Ignore())
                .ForMember(x => x.Tags, x => x.Ignore())
                .ForMember(x => x.Categories, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore());
            Mapper.CreateMap<Contract.Models.NewsItem, NewsItem>()
                .ForMember(x => x.Keywords, x => x.Ignore());

            Mapper.CreateMap<Category, Contract.Models.Category>()
                .ForMember(x => x.ChildrenCategories, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.ParentCategory, x => x.Ignore());

            Mapper.CreateMap<Contract.Models.Category, Category>();

            Mapper.CreateMap<TagStat, Contract.Models.TagStat>();

            Mapper.CreateMap<Data.Models.RssCrawler, Contract.Models.RssCrawler>()
                .ForMember(x => x.Filters, x => x.Ignore())
                .ForMember(x => x.TargetCategory, x => x.Ignore())
                .ForMember(x => x.ExcludeItems, x => x.ResolveUsing(y =>  y.ExcludeTags?.Split(',').Select(z => z.Trim()).Select(z => new RssCrawlerExcludeItem(){RssCrawlerId = y.RssCrawlerId,Selector = z, ExcludeItemIndex = 0}) ?? new List<RssCrawlerExcludeItem>()));
            ;
            Mapper.CreateMap<Contract.Models.RssCrawler, Data.Models.RssCrawler>()
                .ForMember(x => x.ExcludeTags, x => x.ResolveUsing(y => string.Join(", ", y.ExcludeItems.Select(z => z.Selector).Where(z => !string.IsNullOrWhiteSpace(z)).ToArray())));
                
            Mapper.CreateMap<Data.Models.RssCrawlerFilter, Contract.Models.RssCrawlerFilter>();
            Mapper.CreateMap<Contract.Models.RssCrawlerFilter, Data.Models.RssCrawlerFilter>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
