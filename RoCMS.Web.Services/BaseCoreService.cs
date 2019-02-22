using System;
using System.Globalization;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Base.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;
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
            get { return 30; }
        }

        static BaseCoreService()
        {
            ConfigureMapper();
        }

        private static void ConfigureMapper()
        {
            Mapper.CreateMap<Data.Models.HeartState, Contract.Models.HeartState>();
            Mapper.CreateMap<Contract.Models.HeartState, Data.Models.HeartState>();
            Mapper.CreateMap<Data.Models.Heart, Contract.Models.Heart>()
                .ForMember(x => x.CanonicalUrl, x => x.Ignore());
            Mapper.CreateMap<Contract.Models.Heart, Data.Models.Heart>()
                .ForMember(x => x.Type, x => x.MapFrom(y => y.GetType().FullName));

            Mapper.CreateMap<Block, Contract.Models.Block>();
            Mapper.CreateMap<Contract.Models.Block, Block>();
            Mapper.CreateMap<Block, IdNamePair<int>>()
                                            .ForMember(x => x.ID, x => x.MapFrom(m => m.BlockId))
                                            .ForMember(x => x.Name, x => x.MapFrom(m => m.Title));

            Mapper.CreateMap<Page, Contract.Models.Page>()
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
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.Options, x => x.Ignore())
                .ForMember(x => x.State, x => x.Ignore());
            Mapper.CreateMap<Contract.Models.Page, Page>();


            Mapper.CreateMap<Review, Contract.Models.Review>();
            Mapper.CreateMap<Contract.Models.Review, Review>();

            Mapper.CreateMap<Data.Models.Mail, MailMsg>()
                .ForMember(x => x.BccReceiver, x => x.Ignore())
               .ForMember(x => x.AttachIds, x => x.Ignore());


            Mapper.CreateMap<Data.Models.Mail, Mail>();
            Mapper.CreateMap<Mail, Data.Models.Mail>();

            Mapper.CreateMap<User, Contract.Models.User>()
                .ForMember(x => x.Password, x => x.Ignore());

            Mapper.CreateMap<Contract.Models.User, User>();



            Mapper.CreateMap<Contract.Models.Menu, Menu>();
            Mapper.CreateMap<Menu, Contract.Models.Menu>()
                .ForMember(x => x.Items, x => x.Ignore());
            Mapper.CreateMap<Contract.Models.MenuItem, MenuItem>()
                .ForMember(x => x.ParentMenuItemId, x => x.Ignore())
                .ForMember(x => x.MenuId, x => x.Ignore())
                .ForMember(x => x.SortOrder, x => x.Ignore());
            Mapper.CreateMap<MenuItem, Contract.Models.MenuItem>()
                .ForMember(x => x.Items, x => x.Ignore());



            Mapper.CreateMap<Contract.Models.Album, Album>();
            Mapper.CreateMap<Album, Contract.Models.Album>()
                .ForMember(x => x.ImageCount, x => x.Ignore());

            Mapper.CreateMap<Contract.Models.Slider, Slider>();
            Mapper.CreateMap<Slider, Contract.Models.Slider>();
            Mapper.CreateMap<Contract.Models.Slide, Slide>()
                .ForMember(x => x.SortOrder, x => x.Ignore());
            Mapper.CreateMap<Slide, Contract.Models.Slide>();


            Mapper.CreateMap<Data.Models.VideoInfo, VideoInfo>();
            Mapper.CreateMap<VideoInfo, Data.Models.VideoInfo>()
                .ForMember(x => x.AlbumId, x => x.Ignore())
                .ForMember(x => x.SortOrder, x => x.Ignore());

            Mapper.CreateMap<VideoAlbum, Contract.Models.VideoAlbum>()
                .ForMember(x => x.ID, x => x.MapFrom(m => m.AlbumId));
            Mapper.CreateMap<Contract.Models.VideoAlbum, VideoAlbum>()
                .ForMember(x => x.AlbumId, x => x.MapFrom(m => m.ID))
                .ForMember(x => x.CreationDate, x => x.Ignore())
                .ForMember(x => x.OwnerId, x => x.Ignore());

            Mapper.CreateMap<CMSResource, Data.Models.CMSResource>();
            Mapper.CreateMap<Data.Models.CMSResource, CMSResource>();


            Mapper.CreateMap<SearchItem, Data.Models.SearchItem>();
            Mapper.CreateMap<Data.Models.SearchItem, SearchItem>();

            Mapper.CreateMap<Data.Models.SearchResultItem, SearchResultItem>()
                .ForMember(x => x.Relevance, y => y.MapFrom(z => z.Relevance));


            Mapper.CreateMap<Review, Contract.Models.Review>();
            Mapper.CreateMap<Contract.Models.Review, Review>();

            Mapper.CreateMap<InterfaceString, Data.Models.InterfaceString>();
            Mapper.CreateMap<Data.Models.InterfaceString, InterfaceString>();

            Mapper.CreateMap<Data.Models.FormRequestState, FormRequestState>();
            Mapper.CreateMap<FormRequestState, Data.Models.FormRequestState>();

            Mapper.CreateMap<Data.Models.PaymentType, PaymentType>();
            Mapper.CreateMap<PaymentType, Data.Models.PaymentType>();

            Mapper.CreateMap<Data.Models.PaymentState, PaymentState>();
            Mapper.CreateMap<PaymentState, Data.Models.PaymentState>();

            Mapper.CreateMap<Data.Models.FormRequest, FormRequest>();
            Mapper.CreateMap<FormRequest, Data.Models.FormRequest>();

            Mapper.CreateMap<FormRequest, Message>()
               .ForMember(x => x.MessageType, x => x.Ignore())
               .ForMember(x => x.Contact, x => x.Ignore())
               .ForMember(x => x.OrderFormId, x => x.Ignore())
               .ForMember(x => x.Fields, x => x.Ignore())
               .ForMember(x => x.AttachIds, x => x.Ignore());
            Mapper.CreateMap<Message, FormRequest>()
                .ForMember(x => x.FormRequestId, x => x.Ignore())
                .ForMember(x => x.State, x => x.Ignore())
                .ForMember(x => x.PaymentState, x => x.Ignore())
                .ForMember(x => x.CreationDate, x => x.Ignore());



            Mapper.CreateMap<string, OrderFormFieldType>().ConvertUsing((x) =>
            {
                return (OrderFormFieldType)Enum.Parse(typeof(OrderFormFieldType), x);
            });
            Mapper.CreateMap<OrderFormFieldType, string>().ConvertUsing((x) =>
            {
                return Enum.GetName(typeof(OrderFormFieldType), x);
            });

            Mapper.CreateMap<decimal, string>().ConstructUsing(x => x.ToString(CultureInfo.InvariantCulture));
            Mapper.CreateMap<string, decimal>().ConstructUsing(x => decimal.Parse(x, CultureInfo.InvariantCulture));

            Mapper.CreateMap<Data.Models.OrderFormField, OrderFormField>();
            Mapper.CreateMap<OrderFormField, Data.Models.OrderFormField>();

            Mapper.CreateMap<Data.Models.OrderForm, OrderForm>()
                .ForMember(x => x.Fields, x => x.Ignore());
            Mapper.CreateMap<OrderForm, Data.Models.OrderForm>();

            Mapper.AssertConfigurationIsValid();
        }


    }
}
