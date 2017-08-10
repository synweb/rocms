using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Base.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Services;
using Album = RoCMS.Data.Models.Album;
using Block = RoCMS.Data.Models.Block;
using Menu = RoCMS.Data.Models.Menu;
using MenuItem = RoCMS.Data.Models.MenuItem;
using Page = RoCMS.Data.Models.Page;
using Review = RoCMS.Data.Models.Review;
using Slide = RoCMS.Data.Models.Slide;
using Slider = RoCMS.Data.Models.Slider;
using User = RoCMS.Data.Models.User;
using VideoAlbum = RoCMS.Data.Models.VideoAlbum;

namespace RoCMS.Web.Services
{
    public abstract class BaseCoreService : BaseCacheService
    {
        protected override int CacheExpirationInMinutes
        {
            get { return 10; }
        }

        protected static readonly IMapperService _mapper;

        static BaseCoreService()
        {
            _mapper = DependencyResolver.Current.GetService<IMapperService>();
            ConfigureMapper();
        }

        private static void ConfigureMapper()
        {
            _mapper.CreateMap<Page, Contract.Models.Page>((page) =>
            {
                return new Contract.Models.Page()
                {
                    PageId = page.PageId,
                    Annotation = page.Annotation,
                    Content = page.Content,
                    CreationDate = page.CreationDate,
                    ParentPageId = page.ParentPageId,
                    Keywords = page.Keywords,
                    RelativeUrl = page.RelativeUrl,
                    Title = page.Title,
                    HideInSitemap = page.HideInSitemap,
                    Header = page.Header,
                    Scripts = page.Scripts,
                    Styles = page.Styles,
                    Layout = page.Layout,
                    AdditionalHeaders = page.AdditionalHeaders
                };
            });
            _mapper.CreateMap<Contract.Models.Page, Page>();

            Mapper.CreateMap<Block, RoCMS.Web.Contract.Models.Block>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.Block, Block>();
            Mapper.CreateMap<Block, IdNamePair<int>>()
                                            .ForMember(x => x.ID, x => x.MapFrom(m => m.BlockId))
                                            .ForMember(x => x.Name, x => x.MapFrom(m => m.Title));

            Mapper.CreateMap<Data.Models.Page, Contract.Models.Page>()
                .ForMember(x => x.CannonicalUrl, x => x.Ignore());
            Mapper.CreateMap<Data.Models.Page, PageInfo>()
                .ForMember(x => x.CannonicalUrl, x => x.Ignore());
            Mapper.CreateMap<Contract.Models.Page, Data.Models.Page>();

            _mapper.CreateTwoWayMap<Block, Contract.Models.Block>();

            Mapper.CreateMap<Review, Contract.Models.Review>();
            Mapper.CreateMap<Contract.Models.Review, Review>();

            Mapper.CreateMap<RoCMS.Data.Models.Mail, RoCMS.Web.Contract.Models.MailMsg>()
                .ForMember(x => x.BccReceiver, x => x.Ignore())
               .ForMember(x => x.AttachIds, x => x.Ignore());


            Mapper.CreateMap<RoCMS.Data.Models.Mail, RoCMS.Web.Contract.Models.Mail>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.Mail, RoCMS.Data.Models.Mail>();

            Mapper.CreateMap<Data.Models.User, Contract.Models.User>()
                .ForMember(x => x.Password, x => x.Ignore());

            _mapper.CreateMap<Contract.Models.User, User>();



            Mapper.CreateMap<Contract.Models.Menu, Data.Models.Menu>();
            Mapper.CreateMap<Data.Models.Menu, Contract.Models.Menu>()
                .ForMember(x => x.Items, x => x.Ignore());
            Mapper.CreateMap<Contract.Models.MenuItem, Data.Models.MenuItem>()
                .ForMember(x => x.ParentMenuItemId, x => x.Ignore())
                .ForMember(x => x.MenuId, x => x.Ignore())
                .ForMember(x => x.SortOrder, x => x.Ignore());
            Mapper.CreateMap<Data.Models.MenuItem, Contract.Models.MenuItem>()
                .ForMember(x => x.Items, x => x.Ignore());



            Mapper.CreateMap<Contract.Models.Album, Data.Models.Album>();
            Mapper.CreateMap<Data.Models.Album, Contract.Models.Album>()
                .ForMember(x => x.ImageCount, x => x.Ignore());

            Mapper.CreateMap<RoCMS.Web.Contract.Models.Slider, Slider>();
            Mapper.CreateMap<Slider, RoCMS.Web.Contract.Models.Slider>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.Slide, Slide>()
                .ForMember(x => x.SortOrder, x => x.Ignore());
            Mapper.CreateMap<Slide, RoCMS.Web.Contract.Models.Slide>();


            Mapper.CreateMap<Data.Models.VideoInfo, RoCMS.Web.Contract.Models.VideoInfo>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.VideoInfo, Data.Models.VideoInfo>()
                .ForMember(x => x.AlbumId, x => x.Ignore())
                .ForMember(x => x.SortOrder, x => x.Ignore());

            Mapper.CreateMap<Data.Models.VideoAlbum, RoCMS.Web.Contract.Models.VideoAlbum>()
                .ForMember(x => x.ID, x => x.MapFrom(m => m.AlbumId));
            Mapper.CreateMap<RoCMS.Web.Contract.Models.VideoAlbum, Data.Models.VideoAlbum>()
                .ForMember(x => x.AlbumId, x => x.MapFrom(m => m.ID))
                .ForMember(x => x.CreationDate, x => x.Ignore())
                .ForMember(x => x.OwnerId, x => x.Ignore());

            Mapper.CreateMap<CMSResource, Data.Models.CMSResource>();
            Mapper.CreateMap<Data.Models.CMSResource, CMSResource>();


            Mapper.CreateMap<SearchItem, Data.Models.SearchItem>();
            Mapper.CreateMap<Data.Models.SearchItem, SearchItem>();

            Mapper.CreateMap<Data.Models.SearchResultItem, SearchResultItem>()
                .ForMember(x => x.Relevance, y => y.MapFrom(z => z.Relevance));


            Mapper.CreateMap<Review, RoCMS.Web.Contract.Models.Review>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.Review, Review>();

            Mapper.CreateMap<InterfaceString, Data.Models.InterfaceString>();
            Mapper.CreateMap<Data.Models.InterfaceString, InterfaceString>();


            Mapper.CreateMap<RoCMS.Data.Models.FormRequest, RoCMS.Web.Contract.Models.FormRequest>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.FormRequest, RoCMS.Data.Models.FormRequest>();

            Mapper.CreateMap<RoCMS.Web.Contract.Models.FormRequest, RoCMS.Web.Contract.Models.Message>()
               .ForMember(x => x.MessageType, x => x.Ignore())
               .ForMember(x => x.Contact, x => x.Ignore())
               .ForMember(x => x.OrderFormId, x => x.Ignore())
               .ForMember(x => x.Fields, x => x.Ignore())
               .ForMember(x => x.AttachIds, x => x.Ignore());
            Mapper.CreateMap<RoCMS.Web.Contract.Models.Message, RoCMS.Web.Contract.Models.FormRequest>()
                .ForMember(x => x.FormRequestId, x => x.Ignore())
                .ForMember(x => x.CreationDate, x => x.Ignore());


            Mapper.CreateMap<string, RoCMS.Web.Contract.Models.OrderFormFieldType>().ConvertUsing((x) =>
            {
                return (OrderFormFieldType)Enum.Parse(typeof (OrderFormFieldType), x);
            });
            Mapper.CreateMap<RoCMS.Web.Contract.Models.OrderFormFieldType, string>().ConvertUsing((x) =>
            {
                return Enum.GetName(typeof(OrderFormFieldType), x);
            });

            Mapper.CreateMap<RoCMS.Data.Models.OrderFormField, RoCMS.Web.Contract.Models.OrderFormField>();
            Mapper.CreateMap<RoCMS.Web.Contract.Models.OrderFormField, RoCMS.Data.Models.OrderFormField>();

            Mapper.CreateMap<RoCMS.Data.Models.OrderForm, RoCMS.Web.Contract.Models.OrderForm>()
                .ForMember(x => x.Fields, x => x.Ignore());
            Mapper.CreateMap<RoCMS.Web.Contract.Models.OrderForm, RoCMS.Data.Models.OrderForm>();
        }


    }
}
