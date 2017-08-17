using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Services;
using RoCMS.News.Contract.Models;
using Category = RoCMS.News.Data.Models.Category;
using NewsItem = RoCMS.News.Data.Models.NewsItem;
using TagStat = RoCMS.News.Data.Models.TagStat;

namespace RoCMS.News.Services
{
    public abstract class NewsService: BaseCacheService
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
                .ForMember(x => x.Tags, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.Categories, x => x.Ignore())
                ;

            Mapper.CreateMap<Contract.Models.NewsItem, NewsItem>();

            Mapper.CreateMap<Category, Contract.Models.Category>()
                .ForMember(x => x.ChildrenCategories, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.ParentCategory, x => x.Ignore());

            Mapper.CreateMap<Contract.Models.Category, Category>();

            Mapper.CreateMap<TagStat, Contract.Models.TagStat>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
