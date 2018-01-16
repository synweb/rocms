using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;

namespace RoCMS.Shop.Contract.Models
{
    public class GoodsItem: ISearchable
    {
        public GoodsItem()
        {
            this.Images = new List<string>();
            this.Categories = new List<IdNamePair<int>>();
            this.Actions = new List<ActionShortInfo>();
            this.GoodsSpecs = new List<SpecValue>();
            this.Packs = new List<GoodsPack>();
            this.CompatibleGoods = new List<CompatibleSet>();
            //this.GoodsReviews = new List<GoodsReview>();
        }

        public int HeartId { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }

        public bool NotAvailable { get; set; }

        public int? ManufacturerId { get; set; }

        public int? SupplierId { get; set; }

        public int? BasePackId { get; set; }

        /// <summary>
        /// Базовая стоимость за базовую упаковку
        /// </summary>
        public decimal Price { get; set; }

        public decimal MainCurrPrice
        {
            get
            {
                try
                {
                    var currencyService = DependencyResolver.Current.GetService<IShopCurrencyService>();
                    return currencyService.Convert(Price, Currency, "RUR");
                }
                catch
                {
                    return Price;
                }
            }
        }

        public decimal DisplayPrice
        {
            get
            {
                try
                {
                    var currencyService = DependencyResolver.Current.GetService<IShopCurrencyService>();
                    return currencyService.ConvertToDisplayCurrency(Price, Currency);
                }
                catch
                {
                    return Price;
                }
            }
        }

        public bool HasDiscount
        {
            get { return this.Actions.Any(); }
        }


        public int Discount
        {
            get
            {
                if (!HasDiscount)
                {
                    return 0;
                }
                //TODO: пока что скидки не суммируются. Нужно будет сделать настройку в админке, если понадобится
                return this.Actions.Max(x => x.Discount);
            }
        }

        public decimal DiscountedPrice
        {
            get { return Price - Price*(Discount/100m); }
        }

        public decimal MainCurrDiscountedPrice
        {
            get
            {
                try
                {
                    var currencyService = DependencyResolver.Current.GetService<IShopCurrencyService>();
                    return currencyService.Convert(DiscountedPrice, Currency, "RUR");
                }
                catch
                {
                    return DiscountedPrice;
                }
            }
        }

        public decimal DisplayDiscountedPrice
        {
            get
            {
                try
                {
                    var currencyService = DependencyResolver.Current.GetService<IShopCurrencyService>();
                    return currencyService.ConvertToDisplayCurrency(DiscountedPrice, Currency);
                }
                catch
                {
                    return DiscountedPrice;
                }
            }
        }

        public bool HasPacks
        {
            get { return Packs.Any() && BasePack != null; }
        }

        public System.DateTime DateOfAddition { get; set; }

        public string Description { get; set; }
        public string HtmlDescription { get; set; }
        public string MainImageId { get; set; }
        public string Article { get; set; }
        public float? Rating { get; set; }
        public string Currency { get; set; }
        

        public Manufacturer Manufacturer { get; set; }

        public Manufacturer Supplier { get; set; }
        public IList<string> Images { get; set; }
        public IList<IdNamePair<int>> Categories { get; set; }
        public IList<ActionShortInfo> Actions { get; set; }
        public IList<SpecValue> GoodsSpecs { get; set; }
        public Currency Currency1 { get; set; }
        public IList<GoodsPack> Packs { get; set; }

        public Pack BasePack { get; set; }

        public IList<CompatibleSet> CompatibleGoods { get; set; }


        public string CannonicalUrl { get; set; }
        public string Filename { get; set; }

        //public IList<GoodsReview> GoodsReviews { get; set; }
        public decimal PriceForPack(int packId)
        {
            var pack = Packs.First(x => x.PackInfo.PackId == packId);

            //Если базовая упаковка или меньше базовой - не применяем скидку
            bool noPackDiscount = pack.PackInfo.PackId == BasePackId || pack.PackInfo.Size <= BasePack.Size;

            decimal price = pack.Price
                ?? (noPackDiscount ? Price * (decimal)pack.PackInfo.Size / (decimal)BasePack.Size
                : Price * (decimal)pack.PackInfo.Size / (decimal)BasePack.Size *
                            (100m - (decimal)(pack.Discount ?? pack.PackInfo.DefaultDiscount ?? 0)) / 100m);
            return Math.Round(price, 2);
        }

        public decimal DiscountedPriceForPack(int packId)
        {
            decimal packPrice = PriceForPack(packId);
            return Math.Round(packPrice - packPrice*((decimal)Discount/100m), 2);
        }

        

        public decimal MainCurrDiscountedPriceForPack(int packId)
        {

                try
                {
                    var currencyService = DependencyResolver.Current.GetService<IShopCurrencyService>();
                    return currencyService.Convert(DiscountedPriceForPack(packId), Currency, "RUR");
                }
                catch
                {
                    return DiscountedPriceForPack(packId);
                }

        }

        public decimal DisplayDiscountedPriceForPack(int packId)
        {

            try
            {
                var currencyService = DependencyResolver.Current.GetService<IShopCurrencyService>();
                return currencyService.ConvertToDisplayCurrency(DiscountedPriceForPack(packId), Currency);
            }
            catch
            {
                return DiscountedPriceForPack(packId);
            }

        }



        public IEnumerable<string> SearchIndexKeys => new[]
        { SearchKeyName, SearchKeyDescription, SearchKeyHtmlDescription};

        public string SearchKeyName => nameof(Name);
        public string SearchKeyDescription => nameof(Description);
        public string SearchKeyHtmlDescription => nameof(HtmlDescription);
    }
}

