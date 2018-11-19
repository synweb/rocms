using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Base.Services;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Data.Models;
using RoCMS.Shop.Export.Contract.Models;
using RoCMS.Web.Contract.Models;
using Action = RoCMS.Shop.Contract.Models.Action;
using Cart = RoCMS.Shop.Contract.Models.Cart;
using CartItem = RoCMS.Shop.Contract.Models.CartItem;
using Category = RoCMS.Shop.Contract.Models.Category;
using Client = RoCMS.Shop.Contract.Models.Client;
using CompatibleSet = RoCMS.Shop.Contract.Models.CompatibleSet;
using Currency = RoCMS.Shop.Contract.Models.Currency;
using Dimension = RoCMS.Shop.Contract.Models.Dimension;
using FavouriteItem = RoCMS.Shop.Contract.Models.FavouriteItem;
using GoodsFilter = RoCMS.Shop.Contract.Models.GoodsFilter;
using GoodsInOrder = RoCMS.Shop.Data.Models.GoodsInOrder;
using GoodsItem = RoCMS.Shop.Contract.Models.GoodsItem;
using GoodsPack = RoCMS.Shop.Contract.Models.GoodsPack;
using GoodsReview = RoCMS.Shop.Contract.Models.GoodsReview;
using Manufacturer = RoCMS.Shop.Contract.Models.Manufacturer;
using Order = RoCMS.Shop.Contract.Models.Order;
using OrderState = RoCMS.Shop.Contract.Models.OrderState;
using Pack = RoCMS.Shop.Contract.Models.Pack;
using PaymentState = RoCMS.Shop.Contract.Models.PaymentState;
using PaymentType = RoCMS.Shop.Contract.Models.PaymentType;
using ShipmentType = RoCMS.Shop.Contract.Models.ShipmentType;
using Spec = RoCMS.Shop.Contract.Models.Spec;

namespace RoCMS.Shop.Services
{
    public class BaseShopService: BaseCacheService
    {
        static BaseShopService()
        {
            Mapper.CreateMap<Manufacturer, Data.Models.Manufacturer>();
            Mapper.CreateMap<Data.Models.Manufacturer, Manufacturer>()
                .ForMember(x => x.Country, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
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
                .ForMember(x => x.Options, x => x.Ignore())
                .ForMember(x => x.State, x => x.Ignore());

            Mapper.CreateMap<Spec, Data.Models.Spec>();
            Mapper.CreateMap<Data.Models.Spec, Spec>();

            Mapper.CreateMap<RoCMS.Data.Models.Country, Country>(); //TODO: перенести в ядро
            Mapper.CreateMap<Country, RoCMS.Data.Models.Country>();

            Mapper.CreateMap<Category, Data.Models.Category>();
            Mapper.CreateMap<Data.Models.Category, Category>()
                .ForMember(x => x.OrderFormSpecs, x => x.Ignore())
                .ForMember(x => x.ChildrenCategories, x => x.Ignore())
                .ForMember(x => x.ParentCategory, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
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
                .ForMember(x => x.Options, x => x.Ignore())
                .ForMember(x => x.State, x => x.Ignore());

            Mapper.CreateMap<Data.Models.Category, IdNamePair<int>>()
                .ForMember(x => x.ID, x => x.MapFrom(y => y.HeartId))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Name));

            Mapper.CreateMap<PickupPointInfo, PickUpPoint>();
            Mapper.CreateMap<PickUpPoint, PickupPointInfo>();

            Mapper.CreateMap<Dimension, Data.Models.Dimension>();
            Mapper.CreateMap<Data.Models.Dimension, Dimension>();
            Mapper.CreateMap<Pack, Data.Models.Pack>();
            Mapper.CreateMap<Data.Models.Pack, Pack>()
                .ForMember(x => x.Dimension, x => x.Ignore());

            Mapper.CreateMap<CompatibleSet, Data.Models.CompatibleSet>();
            Mapper.CreateMap<Data.Models.CompatibleSet, CompatibleSet>()
                .ForMember(x => x.CompatibleGoods, x => x.Ignore());

            Mapper.CreateMap<GoodsReview, Data.Models.GoodsReview>();
            Mapper.CreateMap<Data.Models.GoodsReview, GoodsReview>()
                .ForMember(x => x.GoodsItem, x => x.Ignore());

            Mapper.CreateMap<GoodsPack, Data.Models.GoodsPack>()
                .ForMember(x => x.PackId, x => x.MapFrom(y => y.PackInfo.PackId));
            Mapper.CreateMap<Data.Models.GoodsPack, GoodsPack>()
                .ForMember(x => x.PackInfo, x => x.Ignore());

            Mapper.CreateMap<SpecValue, GoodsSpec>()
                .ForMember(x => x.SpecId, x => x.MapFrom(y => y.Spec.SpecId));
            Mapper.CreateMap<GoodsSpec, SpecValue>()
                .ForMember(x => x.Spec, x => x.Ignore());

            Mapper.CreateMap<Action, Data.Models.Action>();
            Mapper.CreateMap<Data.Models.Action, Action>()
                .ForMember(x => x.Goods, x => x.Ignore())
                .ForMember(x => x.Manufacturers, x => x.Ignore())
                .ForMember(x => x.Categories, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
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
                .ForMember(x => x.Options, x => x.Ignore())
                .ForMember(x => x.State, x => x.Ignore());

            Mapper.CreateMap<GoodsFilter, Data.Models.GoodsFilter>()
                .ForMember(x => x.WithSubcategories, x => x.MapFrom(y => y.ClientMode??false))
                .ForMember(x => x.SpecIds, x => x.MapFrom(y => y.SpecIdValues))
                .ForMember(x => x.PackIds, x => x.MapFrom(y => y.Packs));

            Mapper.CreateMap<Data.Models.Currency, Currency>();
            Mapper.CreateMap<Currency, Data.Models.Currency>();

            Mapper.CreateMap<RegularCustomerDiscount, RegularClientDiscount>();
            Mapper.CreateMap<RegularClientDiscount, RegularCustomerDiscount>();

            Mapper.CreateMap<Data.Models.Client, Client>()
                .ForMember(x => x.Address,
                    x => x.MapFrom(y => DataContractSerializeHelper.Deserialize<Address>(y.Address) ?? new Address()));
            Mapper.CreateMap<Client, Data.Models.Client>()
                .ForMember(x => x.Address,
                    x => x.MapFrom(y => DataContractSerializeHelper.SerializeToXmlString(y.Address)));


            Mapper.CreateMap<PickUpPoint, PickupPointInfo>();
            Mapper.CreateMap<PickupPointInfo, PickUpPoint>();


            Mapper.CreateMap<GoodsItem, Data.Models.GoodsItem>()
                .ForMember(x => x.Deleted, x => x.Ignore())
                .ForMember(x => x.SearchDescription, x => x.Ignore());

            //TODO: повторяется куча полей в списке
            Mapper.CreateMap<Data.Models.GoodsItem, GoodsItem>()
                .ForMember(x => x.Images, x => x.Ignore())
                .ForMember(x => x.Categories, x => x.Ignore())
                .ForMember(x => x.GoodsSpecs, x => x.Ignore())
                .ForMember(x => x.Packs, x => x.Ignore())
                .ForMember(x => x.CompatibleGoods, x => x.Ignore())
                .ForMember(x => x.Actions, x => x.Ignore())
                .ForMember(x => x.MainCurrPrice, x => x.Ignore())
                .ForMember(x => x.MainCurrDiscountedPrice, x => x.Ignore())
                .ForMember(x => x.DisplayPrice, x => x.Ignore())
                .ForMember(x => x.HasDiscount, x => x.Ignore())
                .ForMember(x => x.Discount, x => x.Ignore())
                .ForMember(x => x.DiscountedPrice, x => x.Ignore())
                .ForMember(x => x.DisplayDiscountedPrice, x => x.Ignore())
                .ForMember(x => x.HasPacks, x => x.Ignore())
                .ForMember(x => x.Manufacturer, x => x.Ignore())
                .ForMember(x => x.Supplier, x => x.Ignore())
                .ForMember(x => x.Currency1, x => x.Ignore())
                .ForMember(x => x.BasePack, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.Rating, x => x.Ignore())
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
                .ForMember(x => x.Categories, x => x.Ignore())
                .ForMember(x => x.CanonicalUrl, x => x.Ignore())
                .ForMember(x => x.Options, x => x.Ignore())
                .ForMember(x => x.State, x => x.Ignore());

            Mapper.CreateMap<CartItem, GoodsInOrder>()
                .ForMember(x => x.OrderId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Price, x => x.MapFrom(y => y.DiscountedPrice))
                .ForMember(x => x.HeartId, x => x.MapFrom(y => y.GoodsItem.HeartId));
            Mapper.CreateMap<CartItem, Contract.Models.GoodsInOrder>()
                .ForMember(x => x.OrderId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Price, x => x.MapFrom(y => y.DiscountedPrice))
                .ForMember(x => x.HeartId, x => x.MapFrom(y => y.GoodsItem.HeartId))
                .ForMember(x => x.Pack, x => x.MapFrom(y => y.Pack.PackInfo))
                .ForMember(x => x.Goods, x => x.MapFrom(y => y.GoodsItem));

            Mapper.CreateMap<RoCMS.Shop.Contract.Models.GoodsInOrder, GoodsInOrder>();
            Mapper.CreateMap<GoodsInOrder, RoCMS.Shop.Contract.Models.GoodsInOrder>()
                .ForMember(x => x.Pack, x => x.Ignore())
                .ForMember(x => x.Goods, x => x.Ignore());

            Mapper.CreateMap<CartItem, Data.Models.CartItem>();
            Mapper.CreateMap<Data.Models.CartItem, CartItem>()
                .ForMember(x => x.GoodsItem, x => x.Ignore());

            Mapper.CreateMap<FavouriteItem, Data.Models.FavouriteItem>();
            Mapper.CreateMap<Data.Models.FavouriteItem, FavouriteItem>()
                .ForMember(x => x.Item, x => x.Ignore());

            Mapper.CreateMap<Cart, Data.Models.Cart>();
            Mapper.CreateMap<Data.Models.Cart, Cart>()
                .ForMember(x => x.CartItems, x => x.Ignore());

            Mapper.CreateMap<Order, Data.Models.Order>()
                .ForMember(x => x.Address, x => x.ResolveUsing(y =>
                {
                    var address = new Address()
                    {
                        Appartment = y.Appartment,
                        City = y.City,
                        Street = y.Street,
                        FrontNumber = y.FrontNumber,
                        House = y.House,
                        HouseIndex = y.HouseIndex,
                        Metro = y.Metro,
                        PostCode = y.PostCode,
                        Floor = y.Floor,
                        Intercom = y.Intercom
                    };
                    string addressString = DataContractSerializeHelper.SerializeToXmlString(address);
                    return addressString;
                }));

            Mapper.CreateMap<Data.Models.Order, Order>()
                .ConvertUsing(order => {
                    var address = DataContractSerializeHelper.Deserialize<Address>(order.Address);
                    ShipmentType st = Mapper.Map<ShipmentType>(order.ShipmentType);
                    OrderState state = Mapper.Map<OrderState>(order.State);
                    PaymentType pt = Mapper.Map<PaymentType>(order.PaymentType);
                    PaymentState? ps = order.PaymentState.HasValue
                        ? (PaymentState?)Mapper.Map<PaymentState>(order.PaymentState.Value)
                        : null;

                    var result = new Order()
                    {
                        ClientId = order.ClientId,
                        Comment = order.Comment,
                        AdminComment = order.AdminComment,
                        CreationDate = order.CreationDate,
                        OrderId = order.OrderId,
                        ShipmentDate = order.ShipmentDate,
                        ShipmentType = st,
                        State = state,
                        PickUpPointId = order.PickUpPointId,
                        DeliveryPrice = order.DeliveryPrice,
                        TotalDiscount = order.TotalDiscount,
                        PaymentType = pt,
                        PaymentState = ps
                    };
                    if (address != null)
                    {
                        result.Appartment = address.Appartment;
                        result.City = address.City;
                        result.Street = address.Street;
                        result.FrontNumber = address.FrontNumber;
                        result.House = address.House;
                        result.HouseIndex = address.HouseIndex;
                        result.Metro = address.Metro;
                        result.PostCode = address.PostCode;
                        result.Floor = address.Floor;
                        result.Intercom = address.Intercom;
                    }
                    return result;
                });

            Mapper.CreateMap<ExportTask, ShopDbExportTask>();
            Mapper.CreateMap<ShopDbExportTask, ExportTask>();

            Mapper.CreateMap<Contract.Models.MassPriceChangeTask, Data.Models.MassPriceChangeTask>();
            Mapper.CreateMap<Data.Models.MassPriceChangeTask, Contract.Models.MassPriceChangeTask>();
            Mapper.AssertConfigurationIsValid();
        }

        protected override int CacheExpirationInMinutes => 60;
    }
}
