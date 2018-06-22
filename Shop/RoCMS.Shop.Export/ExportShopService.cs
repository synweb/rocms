using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Export.Contract;
using RoCMS.Shop.Export.Contract.Models;
using RoCMS.Shop.Services;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Export
{
    public class ExportShopService : BaseShopService, IExportShopService
    {
        private readonly ShopDbExportTaskGateway _exportTaskGateway = new ShopDbExportTaskGateway();
        private ILogService _logService;
        private ISettingsService _settingsService;
        private IShopService _shopService;
        private IShopCategoryService _shopCategoryService;
        private IHeartService _heartService;

        public ExportShopService(ISettingsService settingsService, ILogService logService, IShopService shopService, IShopCategoryService shopCategoryService, IHeartService heartService)
        {
            _settingsService = settingsService;
            _logService = logService;
            _shopService = shopService;
            _shopCategoryService = shopCategoryService;
            _heartService = heartService;
        }

        private void GenerateYmlFile(string filePath, int taskId)
        {
            ExportStatus status;
            Guid? errorCode = null;
            try
            {
                var settings = GetYmlExportSettings();

                string siteUrl = settings.SiteUrl;
                if (siteUrl.EndsWith("/"))
                {
                    siteUrl = settings.SiteUrl = siteUrl.Remove(siteUrl.Length - 1);
                }

                XmlDocument resultDocument = new XmlDocument();

                //yml_catalog
                XmlNode root = resultDocument.CreateElement("yml_catalog");
                resultDocument.AppendChild(root);

                XmlAttribute currentAttribute = resultDocument.CreateAttribute("date");
                currentAttribute.Value = DateTime.UtcNow.AddHours(4).ToString("yyyy-MM-dd HH:mm");
                root.Attributes.Append(currentAttribute); // добавляем атрибут

                //shop
                XmlNode shopElement = resultDocument.CreateElement("shop");
                root.AppendChild(shopElement);

                //<name>Magazin</name>
                //<company>Magazin</company>
                //<url>http://www.magazin.ru/</url>
                //<currencies>
                //<currency id="RUR" rate="1" plus="0"/>
                //</currencies>

                XmlNode currentElement = resultDocument.CreateElement("name");
                currentElement.InnerText = settings.SiteName;
                shopElement.AppendChild(currentElement);

                currentElement = resultDocument.CreateElement("company");
                currentElement.InnerText = settings.SiteDescription;
                shopElement.AppendChild(currentElement);

                currentElement = resultDocument.CreateElement("url");
                currentElement.InnerText = siteUrl;
                shopElement.AppendChild(currentElement);

                XmlNode currenciesElement = resultDocument.CreateElement("currencies");
                shopElement.AppendChild(currenciesElement);

                currentElement = resultDocument.CreateElement("currency");

                currentAttribute = resultDocument.CreateAttribute("id");
                currentAttribute.Value = "RUR";
                currentElement.Attributes.Append(currentAttribute);

                currentAttribute = resultDocument.CreateAttribute("rate");
                currentAttribute.Value = "1";
                currentElement.Attributes.Append(currentAttribute);

                currentAttribute = resultDocument.CreateAttribute("plus");
                currentAttribute.Value = "0";
                currentElement.Attributes.Append(currentAttribute);

                currenciesElement.AppendChild(currentElement);


                //<categories>
                //<category id="1">Оргтехника</category>
                //<category id="10" parentId="1">Принтеры</category>
                XmlNode categoriesElement = resultDocument.CreateElement("categories");
                shopElement.AppendChild(categoriesElement);

                WriteCategories(resultDocument, categoriesElement);

                //<local_delivery_cost>300</local_delivery_cost>
                currentElement = resultDocument.CreateElement("local_delivery_cost");
                currentElement.InnerText = settings.DeliveryCost.ToString();
                shopElement.AppendChild(currentElement);


                XmlNode offersElement = resultDocument.CreateElement("offers");
                shopElement.AppendChild(offersElement);

                WriteGoods(resultDocument, offersElement, settings);

                var splittedPath = filePath.Split('\\');
                var dir = string.Join("\\", splittedPath.Except(new[] {splittedPath.Last()}));
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    _logService.TraceMessage($"Created directory {dir}");
                }
                resultDocument.Save(filePath);
                status = ExportStatus.Success;
            }
            catch (Exception e)
            {
                errorCode = _logService.LogError(e);
                status = ExportStatus.Error;
            }

            // task ended
            var dataTask = _exportTaskGateway.SelectOne(taskId);
            dataTask.Status = status.ToString();
            dataTask.ErrorCode = errorCode.ToString();
            dataTask.EndDate = DateTime.UtcNow.ApplySiteTimezone();
            _exportTaskGateway.Update(dataTask);
        }

        public ExportTask StartYmlExportTask(YmlExportSettings settings)
        {
            var task = new ExportTask() { StartDate = DateTime.UtcNow.ApplySiteTimezone(), Status = ExportStatus.Processing };
            var dataTask = Mapper.Map<Data.Models.ShopDbExportTask>(task);
            int id = _exportTaskGateway.Insert(dataTask);
            task.TaskId = dataTask.TaskId = id;
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/shop.yml");
            Task t = new Task(() => GenerateYmlFile(filePath, id));
            t.Start();
            return task;
        }

        public List<ExportTask> GetYmlTasks(int count)
        {
            var dataRes = _exportTaskGateway.Select().Take(count);
            // можно оптимизировать, если вытаскивать количество прямо в хранимке. сейчас вытаскиваются все.
            return Mapper.Map<List<ExportTask>>(dataRes);
        }

        public string GetYmlFileContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/shop.yml");
            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        public YmlExportSettings GetYmlExportSettings()
        {
            string siteName = _settingsService.GetSettings<string>("SiteName");
            string siteDescription = _settingsService.GetSettings<string>("SiteDescription");
            string siteUrl = _settingsService.GetSettings<string>("SiteUrl");
            decimal deliveryCost = _settingsService.GetSettings<decimal>("DeliveryCost");
            decimal rateClick = _settingsService.GetSettings<decimal>("ClickRate");
            bool pickup = _settingsService.GetSettings<bool>("Pickup");

            return new YmlExportSettings()
            {
                DeliveryCost = deliveryCost,
                Pickup = pickup,
                ClickRate = rateClick,
                SiteDescription = siteDescription,
                SiteName = siteName,
                SiteUrl = siteUrl
            };
        }

        public void UpdateYmlExportSettings(YmlExportSettings settings)
        {
            _settingsService.Set("SiteName", settings.SiteName);
            _settingsService.Set("SiteDescription", settings.SiteDescription);
            _settingsService.Set("SiteUrl", settings.SiteUrl);
            _settingsService.Set("ClickRate", settings.ClickRate);
            _settingsService.Set("Pickup", settings.Pickup);
            _settingsService.Set("DeliveryCost", settings.DeliveryCost);
        }

        private void WriteGoods(XmlDocument resultDocument, XmlNode offersElement, YmlExportSettings settings)
        {
            decimal bid = settings.ClickRate;
            string siteUrl = settings.SiteUrl;
            int total;
            FilterCollections filterCollections;
            var goods = _shopService.GetGoodsSet(
                new GoodsFilter()
                {
                    SortBy = SortCriterion.CreationDateAsc,
                    ClientMode = true,
                }, 1, int.MaxValue,
                out total, out filterCollections, true);
            foreach (var goodsItem in goods)
            {
                try
                {
                    if (!goodsItem.Categories.Any()) continue;
                    if(goodsItem.NotAvailable) continue;


                    if (goodsItem.Packs.Any())
                    {
                        foreach (var pack in goodsItem.Packs)
                        {
                            AddElementToYml(resultDocument, offersElement, bid, siteUrl, settings, goodsItem, pack);
                        }
                    }
                    else
                    {
                        AddElementToYml(resultDocument, offersElement, bid, siteUrl, settings, goodsItem);
                    }
                }
                catch (Exception e)
                {
                    _logService.LogError(e, "Ошибка при экспорте товара ID:" + goodsItem.HeartId);
                }
            }


            #region Пример товара

            //            <offer id="12341" type="vendor.model" available="true" bid="13">
            //    <url>http://best.seller.ru/product_page.asp?pid=12344</url>
            //    <price>16800</price>
            //    <currencyId>USD</currencyId>
            //    <categoryId>6</categoryId>
            //    <picture>http://best.seller.ru/img/device12345.jpg</picture>
            //    <store>false</store>
            //    <pickup>false</pickup>
            //    <delivery>true</delivery>
            //    <local_delivery_cost>300</local_delivery_cost>
            //    <typePrefix>Принтер</typePrefix>
            //    <vendor>НP</vendor>
            //    <vendorCode>CH366C</vendorCode>
            //    <model>Deskjet D2663</model>
            //    <description>Серия принтеров для людей, которым нужен надежный, простой в использовании 
            //    цветной принтер для повседневной печати. Формат А4. Технология печати: 4-цветная термальная струйная. 
            //    Разрешение при печати: 4800х1200 т/д.
            //    </description>
            //    <sales_notes>Необходима предоплата.</sales_notes>
            //    <manufacturer_warranty>true</manufacturer_warranty>
            //    <seller_warranty>P3Y</seller_warranty>
            //    <country_of_origin>Япония</country_of_origin>
            //    <barcode>1234567890120</barcode>
            //    <cpa>1</cpa>
            //    <rec>123123,1214,243</rec>
            //    <expiry>P5Y</expiry>
            //    <weight>2.07</weight>
            //    <dimensions>100/25.45/11.112</dimensions>
            //    <param name="Максимальный формат">А4</param>
            //    <param name="Технология печати">термическая струйная</param>
            //    <param name="Тип печати">Цветная</param>
            //    <param name="Количество страниц в месяц" unit="стр">1000</param>
            //    <param name="Потребляемая мощность" unit="Вт">20</param>
            //    <param name="Вес" unit="кг">2.73</param>
            //</offer>

            #endregion
        }


        private void AddElementToYml(XmlDocument resultDocument, XmlNode offersElement, decimal bid, string siteUrl, YmlExportSettings settings, GoodsItem goodsItem, GoodsPack goodsPack = null)
        {
            XmlNode offerElement = resultDocument.CreateElement("offer");
            offersElement.AppendChild(offerElement);

            XmlAttribute currentAttribute = resultDocument.CreateAttribute("id");
            currentAttribute.Value = goodsPack == null ? goodsItem.HeartId.ToString() : $"{goodsItem.HeartId}p{goodsPack.PackInfo.Size}";
            offerElement.Attributes.Append(currentAttribute);

            currentAttribute = resultDocument.CreateAttribute("available");
            currentAttribute.Value = "true";
            offerElement.Attributes.Append(currentAttribute);

            if (bid != 0)
            {
                currentAttribute = resultDocument.CreateAttribute("bid");
                currentAttribute.Value = bid.ToString(CultureInfo.InvariantCulture);
                offerElement.Attributes.Append(currentAttribute);
            }

            var url = _heartService.GetCanonicalUrl(goodsItem.HeartId);

            if (goodsPack != null)
            {
                url = $"{url}#p{goodsPack.PackInfo.Size}";
            }

            XmlNode currentElement = resultDocument.CreateElement("url");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = $"{siteUrl}/{url}";

            decimal price = goodsPack == null
                ? goodsItem.DiscountedPrice
                : goodsItem.DiscountedPriceForPack(goodsPack.PackId);

            currentElement = resultDocument.CreateElement("price");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = price.ToString(CultureInfo.InvariantCulture);

            decimal oldPrice = goodsPack == null
                ? goodsItem.Price
                : goodsItem.PriceForPack(goodsPack.PackId);

            if (oldPrice > price)
            {
                currentElement = resultDocument.CreateElement("oldprice");
                offerElement.AppendChild(currentElement);
                currentElement.InnerText = oldPrice.ToString(CultureInfo.InvariantCulture);
            }

            currentElement = resultDocument.CreateElement("currencyId");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = "RUR";

            currentElement = resultDocument.CreateElement("categoryId");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText =
                (goodsItem.ParentHeartId ?? goodsItem.Categories.First().ID).ToString();

            if (!string.IsNullOrEmpty(goodsItem.MainImageId))
            {
                currentElement = resultDocument.CreateElement("picture");
                offerElement.AppendChild(currentElement);
                currentElement.InnerText = $"{siteUrl}/Gallery/Image/{goodsItem.MainImageId}";
            }
            currentElement = resultDocument.CreateElement("pickup");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = settings.Pickup.ToString().ToLower();

            currentElement = resultDocument.CreateElement("delivery");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = "true";

            currentElement = resultDocument.CreateElement("name");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = goodsPack == null ? goodsItem.Name : $"{goodsItem.Name} {goodsPack.PackInfo.Name}";


            if (goodsItem.ManufacturerId.HasValue)
            {
                currentElement = resultDocument.CreateElement("vendor");
                offerElement.AppendChild(currentElement);
                currentElement.InnerText = goodsItem.Manufacturer.Name;
            }

            currentElement = resultDocument.CreateElement("description");
            offerElement.AppendChild(currentElement);
            currentElement.InnerText = goodsItem.Description;

            // Код ниже неверен
            if (goodsItem.CompatibleGoods.Any())
            {
                List<int> ids = new List<int>();
                foreach (var set in goodsItem.CompatibleGoods)
                {
                    foreach (var pair in set.CompatibleGoods)
                    {
                        ids.Add(pair.ID);
                    }
                }
                ids = ids.Distinct().ToList();
                string recs = String.Join(",", ids.Distinct());
                currentElement = resultDocument.CreateElement("rec");
                offerElement.AppendChild(currentElement);
                currentElement.InnerText = recs;
            }


            foreach (var specVal in goodsItem.GoodsSpecs)
            {
                currentElement = resultDocument.CreateElement("param");
                offerElement.AppendChild(currentElement);
                currentElement.InnerText = specVal.Value;

                currentAttribute = resultDocument.CreateAttribute("name");
                currentAttribute.Value = specVal.Spec.Name;
                currentElement.Attributes.Append(currentAttribute);

                if (!String.IsNullOrEmpty(specVal.Spec.Postfix))
                {
                    currentAttribute = resultDocument.CreateAttribute("unit");
                    currentAttribute.Value = specVal.Spec.Postfix;
                    currentElement.Attributes.Append(currentAttribute);
                }

            }

            if (goodsPack != null)
            {
                currentElement = resultDocument.CreateElement("param");
                offerElement.AppendChild(currentElement);
                currentElement.InnerText = goodsPack.PackInfo.Size.ToString(CultureInfo.InvariantCulture);

                currentAttribute = resultDocument.CreateAttribute("name");
                currentAttribute.Value = "Вес";
                currentElement.Attributes.Append(currentAttribute);

                currentAttribute = resultDocument.CreateAttribute("unit");
                currentAttribute.Value = goodsPack.PackInfo.Dimension.Short;
                currentElement.Attributes.Append(currentAttribute);
            }

        }


        private void WriteCategories(XmlDocument resultDocument, XmlNode categoriesElement)
        {
            {
                var categories = _shopCategoryService.GetAllCategories();
                foreach (var cat in categories)
                {
                    XmlNode currentElement = resultDocument.CreateElement("category");
                    currentElement.InnerText = cat.Name;

                    XmlAttribute currentAttribute = resultDocument.CreateAttribute("id");
                    currentAttribute.Value = cat.HeartId.ToString();
                    currentElement.Attributes.Append(currentAttribute);

                    if (cat.ParentCategoryId.HasValue)
                    {
                        currentAttribute = resultDocument.CreateAttribute("parentId");
                        currentAttribute.Value = cat.ParentCategoryId.Value.ToString();
                        currentElement.Attributes.Append(currentAttribute);
                    }

                    categoriesElement.AppendChild(currentElement);
                }
            }
        }

        protected override int CacheExpirationInMinutes => 60;
    }
}
