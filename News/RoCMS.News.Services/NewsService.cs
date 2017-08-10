﻿using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Services;
using RoCMS.News.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Services
{
    public abstract class NewsService: BaseCacheService
    {
        static NewsService()
        {
            ConfigureMapper();
        }

        protected override int CacheExpirationInMinutes
        {
            get { return AppSettingsHelper.HoursToExpireCartCache * 60; }
        }

        private static void ConfigureMapper()
        {
            Mapper.CreateMap<Blog, Data.Models.Blog>();
            Mapper.CreateMap<Data.Models.Blog, Blog>();

            Mapper.CreateMap<RecordType, Data.Models.RecordType>();
            Mapper.CreateMap<Data.Models.RecordType, RecordType>();
            Mapper.CreateMap<Data.Models.NewsItem, RoCMS.News.Contract.Models.NewsItem>()
                .ForMember(x => x.Tags, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.Categories, x => x.Ignore())
                ;

            Mapper.CreateMap<Contract.Models.NewsItem, Data.Models.NewsItem>();

            Mapper.CreateMap<Data.Models.Category, RoCMS.News.Contract.Models.Category>()
                .ForMember(x => x.ChildrenCategories, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.ParentCategory, x => x.Ignore());

            Mapper.CreateMap<RoCMS.News.Contract.Models.Category, Data.Models.Category>();

            Mapper.CreateMap<Data.Models.TagStat, RoCMS.News.Contract.Models.TagStat>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
