﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using AngleSharp;
using AngleSharp.Extensions;
using AngleSharp.Html;
using AngleSharp.Parser.Html;
using AutoMapper;
using Quartz;
using Quartz.Impl;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Services
{
    public class RssCrawlingService : NewsService, IRssCrawlingService
    {
        private readonly RssCrawlerGateway _rssCrawlerGateway = new RssCrawlerGateway();
        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly RssCrawlerFilterGateway _rssCrawlerFilterGateway = new RssCrawlerFilterGateway();
        private readonly RssProcessedItemGateway _rssProcessedItemGateway = new RssProcessedItemGateway();
        private readonly INewsItemService _newsItemService;
        private readonly IAlbumService _albumService;
        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;
        private readonly ILogService _logService;
        private readonly IHeartService _heartService;

        public RssCrawlingService(INewsItemService newsItemService, IAlbumService albumService, ISettingsService settingsService, IImageService imageService, ILogService logService, IHeartService heartService)
        {
            _newsItemService = newsItemService;
            _albumService = albumService;
            _settingsService = settingsService;
            _imageService = imageService;
            _logService = logService;
            _heartService = heartService;
        }

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

        private void Fill(RssCrawler rssCrawler)
        {
            var dataFilters = _rssCrawlerFilterGateway.SelectByRssCrawler(rssCrawler.RssCrawlerId);
            var filters = Mapper.Map<ICollection<RssCrawlerFilter>>(dataFilters);
            rssCrawler.Filters = filters;
            //rssCrawler.ExcludeItems = //TODO 

            if (rssCrawler.TargetCategoryId != null)
            {
                var cat = _categoryGateway.SelectOne(rssCrawler.TargetCategoryId.Value);
                rssCrawler.TargetCategory = new IdNamePair<int>(cat.CategoryId, cat.Name);
            }
        }

        public void UpdateCrawlers(ICollection<RssCrawler> crawlers)
        {
            var existingCrawlers = _rssCrawlerGateway.Select();
            var dataRecs = Mapper.Map<ICollection<Data.Models.RssCrawler>>(crawlers);
            var comparer = new Func<Data.Models.RssCrawler, Data.Models.RssCrawler, bool>((x, y) => x.RssCrawlerId == y.RssCrawlerId);

            CollectionMergeHelper.MergeNewAndOld(
                newItems: dataRecs,
                existingItems: existingCrawlers,
                comparer: comparer,
                create: (x) =>
                {
                    int id = _rssCrawlerGateway.Insert(x);
                    var dataFilters = Mapper.Map<ICollection<Data.Models.RssCrawlerFilter>>(
                        crawlers.Single(y => x.RssCrawlerId == y.RssCrawlerId).Filters);
                    foreach (var filter in dataFilters)
                    {
                        filter.RssCrawlerId = id;
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


        private const string FEED = "feed";
        private const string ALBUM_SERVICE = "albumService";
        private const string IMAGE_SERVICE = "imageService";
        private const string NEWS_ITEM_SERVICE = "newsItemService";
        private const string SETTINGS_SERVICE = "settingsService";
        private const string LOG_SERVICE = "logService";
        private const string HEART_SERVICE = "heartService";
        private const string RSS_PROCESSED_ITEM_GATEWAY = "RssProcessedItemGateway";

        public void StartCrawling()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            var feeds = GetCrawlers().Where(x => x.IsEnabled);
            foreach (var feed in feeds)
            {
                var job = JobBuilder.Create<RssCrawlerJob>();
                job.SetJobData(new JobDataMap()
                {
                    {FEED, feed},
                    {ALBUM_SERVICE, _albumService},
                    {IMAGE_SERVICE, _imageService},
                    {NEWS_ITEM_SERVICE, _newsItemService},
                    {SETTINGS_SERVICE, _settingsService},
                    {LOG_SERVICE, _logService},
                    {HEART_SERVICE, _heartService},
                    {RSS_PROCESSED_ITEM_GATEWAY, _rssProcessedItemGateway},
                });
                var builtJob = job.Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"rss-rrawling-trigger-{feed.RssCrawlerId}", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x =>
                        x.WithIntervalInMinutes(feed.CheckInterval)
                            .RepeatForever())
                    .Build();
                scheduler.ScheduleJob(builtJob, trigger);
            }
        }

        private class RssCrawlerJob : IJob
        {
            private static string GetRssItemExtensionValue(SyndicationItem item, string extensionName)
            {
                if (item.ElementExtensions.Count == 0)
                    return null; // расширений нет, получать нечего
                var ext = item.ElementExtensions.FirstOrDefault(x =>
                    x.OuterName.Equals(extensionName, StringComparison.InvariantCultureIgnoreCase));
                if (ext == null)
                    return null; // нужного расширения нет, получать нечего
                return
                    item.ElementExtensions.ReadElementExtensions<string>(ext.OuterName,
                        ext.OuterNamespace).FirstOrDefault();
            }


            public async void Execute(IJobExecutionContext context)
            {
                var dataMap = context.JobDetail.JobDataMap;
                var logService = (ILogService) dataMap[LOG_SERVICE];
                try
                {
                    var feed = (RssCrawler) dataMap[FEED];
                    var albumService = (IAlbumService) dataMap[ALBUM_SERVICE];
                    var imageService = (IImageService) dataMap[IMAGE_SERVICE];
                    var newsItemService = (INewsItemService) dataMap[NEWS_ITEM_SERVICE];
                    var settingsService = (ISettingsService) dataMap[SETTINGS_SERVICE];
                    var heartService = (IHeartService)dataMap[HEART_SERVICE];
                    var rssProcessedItemGateway = (RssProcessedItemGateway)dataMap[RSS_PROCESSED_ITEM_GATEWAY];

                    logService.TraceMessage($"Starting crawling {feed.RssFeedUrl}");
                    var feedUrl = feed.RssFeedUrl.StartsWith("//") ? $"https:{feed.RssFeedUrl}" : feed.RssFeedUrl;
                    XmlReader reader = XmlReader.Create(feedUrl);
                    SyndicationFeed syndicationFeed = SyndicationFeed.Load(reader);
                    logService.TraceMessage($"Got {syndicationFeed.Items.Count()} items");
                    foreach (SyndicationItem item in syndicationFeed.Items)
                    {
                         if (!CheckIfItemIsNew(item, rssProcessedItemGateway))
                            continue;

                        // проверка текста по фильтрам
                        string title = item.Title.Text.Trim();
                        string description = ParsingHelper.RemoveHtml(item.Summary.Text).Trim();
                        string fulltext = ParsingHelper.RemoveHtml(GetRssItemExtensionValue(item, "fulltext"));
                        bool filterOk = true;
                        foreach (var filter in feed.Filters)
                        {
                            // фильтры реализованы по логическому "И"
                            // пройдут только те записи, которые соответствуют всем фильтрам
                            var titleMatches = Regex.IsMatch(title, filter.Filter);
                            var descriptionMatches = Regex.IsMatch(description, filter.Filter);
                            var fulltextMatches = fulltext != null && Regex.IsMatch(fulltext, filter.Filter);
                            // если совпадение по регулярке найдено в заголовке, описании или полном тексте, считаем, что фильтр пройден
                            var match = (titleMatches || descriptionMatches || fulltextMatches);
                            filterOk &= match;
                            if (!match)
                                break;
                        }
                        logService.TraceMessage(
                            $@"Item with title ""{item.Title.Text}"" {(filterOk ? "matches" : "does not match")}");

                        Data.Models.RssProcessedItem rssProcessedItem = new Data.Models.RssProcessedItem
                        {
                            NewsItemId = null,
                            RssSource = item.Id,
                        };
                        if (!filterOk)
                        {
                            rssProcessedItemGateway.Insert(rssProcessedItem);
                            continue;
                        }
                        var newsItemTextStringBuilder = new StringBuilder();
                        // html parsing
                        var config = Configuration.Default.WithDefaultLoader();
                        var address = item.Links.First().Uri;
                        var document = await BrowsingContext.New(config).OpenAsync(address.ToString());
                        string imageId = null;
                        if (!string.IsNullOrEmpty(feed.ImageSelector))
                        {
                            // есть селектор для парсинга картинок
                            // Asynchronously get the document in a new context using the configuration
                            // Perform the query to get all cells with the content
                            var cell = document.QuerySelector(feed.ImageSelector);
                            if (cell != null)
                            {
                                
                                var url = cell.Attributes["src"].Value;
                                if (url.StartsWith("/") && url[1] != '/')
                                {
                                    var uri = new Uri(reader.BaseURI);
                                    var feedRootUrl = uri.AbsoluteUri.Replace(uri.AbsolutePath, "");
                                    var endsWithSlash = feedRootUrl.EndsWith("/");
                                    url = $"{feedRootUrl}{(endsWithSlash?"":"/")}{url}";
                                }
                                if (url.StartsWith("//"))
                                {
                                    // ссылку вида //site.ru/img.jpg преобразуем в http://site.ru/img.jpg
                                    url = $"http:{url}";
                                }
                                logService.TraceMessage($"Starting downloading image {url}");
                                imageId = await albumService.DownloadImage(url);
                                logService.TraceMessage($"Downloading image {url} OK");
                            }
                        }

                        if (string.IsNullOrEmpty(feed.ContentContainerSelector))
                        {
                            newsItemTextStringBuilder.Append(item.Summary.Text.Trim());
                        }
                        else
                        {
                            var content = document.QuerySelector(feed.ContentContainerSelector).InnerHtml;
                            // удаляем все изображения
                            var images = document.QuerySelectorAll($"{feed.ContentContainerSelector} img");
                            foreach (var img in images)
                            {
                                content = content.Replace(img.OuterHtml, "");
                            }
                            content = content.Replace("<p></p>", "").Trim();
                            newsItemTextStringBuilder.Append(content);
                        }
                        lock (feed)
                        {
                            if (!CheckIfItemIsNew(item, rssProcessedItemGateway))
                            {
                                // если вдруг за время скачивания картинки другой поток успел добавить эту же новость
                                imageService.RemoveImage(imageId);
                                continue;
                            }

                            const int NEWS_DESCRIPTION_LENGTH = 200;
                            var cuttedDescription = TextCutHelper.Cut(description, NEWS_DESCRIPTION_LENGTH).Trim();
                            bool translitUrls = settingsService.GetSettings<bool>(nameof(Setting.TranslitEnabled));
                            var relativeUrl = translitUrls
                                ? FormattingHelper.ToTranslitedUrl(title)
                                : FormattingHelper.ToRussianURL(title);
                            if (item.Links.Any())
                            {
                                newsItemTextStringBuilder.Append("<br>");
                                newsItemTextStringBuilder.Append("<br>");
                                var linkText = !string.IsNullOrEmpty(feed.LinkText) ? feed.LinkText : "Читать в источнике";
                                newsItemTextStringBuilder.Append($@"<a href=""{item.Links.First().Uri.AbsoluteUri}"" target=""_blank"">{linkText}</a>");
                            }
                            var newsItemText = newsItemTextStringBuilder.ToString();
                            if (feed.ExcludeItems.Any())
                            {
                                newsItemText = RemoveDomElements(newsItemText, feed.ExcludeItems.Select(x => x.Selector), logService);
                            }

                            var blogUrl = settingsService.GetSettings<string>(nameof(NewsSettings.BlogUrl));
                            var parentHeart = heartService.GetHeart(blogUrl);
                            var newNewsItem = new NewsItem()
                            {
                                RssSource = item.Id,
                                AuthorId = null,
                                BreadcrumbsTitle = title,
                                Text = newsItemText,
                                ImageId = imageId,
                                PostingDate = item.PublishDate.DateTime,
                                Description = cuttedDescription,
                                MetaDescription = cuttedDescription,
                                Title = title,
                                RecordType = RecordType.Default,
                                Layout = "clientLayout",
                                RelativeUrl = relativeUrl,
                                ParentHeartId = parentHeart?.HeartId,
                                Tags = feed.Tags
                            };
                            if (feed.TargetCategoryId != null)
                            {
                                newNewsItem.Categories = new List<IdNamePair<int>>()
                                {
                                    new IdNamePair<int>(feed.TargetCategoryId.Value, string.Empty)
                                };
                            }
                            logService.TraceMessage($"Creating NewsItem for {item.Id}");
                            int newsItemId = newsItemService.CreateNewsItem(newNewsItem);
                            rssProcessedItem.NewsItemId = newsItemId;
                            rssProcessedItemGateway.Insert(rssProcessedItem);
                        }
                    }
                }
                catch (Exception e)
                {
                    logService.LogError(e);
                }
            }

            private static IMarkupFormatter _prettyMarkupFormatter = new PrettyMarkupFormatter();

            private string RemoveDomElements(string input, IEnumerable<string> selectors, ILogService logger)
            {
                // html parsing
                try
                {
                    var document = new HtmlParser().Parse(input);
                    foreach (var selector in selectors)
                    {

                        try
                        {
                            var foundElements = document.QuerySelectorAll(selector);
                    
                            foreach (var foundElement in foundElements)
                            {
                                try
                                {
                                    foundElement.Parent.RemoveElement(foundElement);
                                    //document.RemoveElement(foundElement);
                                    var sb = new StringBuilder();
                                    using (var sw = new StringWriter(sb))
                                    {
                                        document.ToHtml(sw, _prettyMarkupFormatter);
                                    }

                                    input = sb.ToString();
                                }
                                catch (Exception e)
                                {
                                    logger.LogError(e);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e);
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e);
                }
                return input;
            }

            private static bool CheckIfItemIsNew(SyndicationItem item, RssProcessedItemGateway rssProcessedItemGateway)
            {
                var processedItem = rssProcessedItemGateway.SelectByRssSource(item.Id);
                // new if item is null
                return processedItem == null;
            }
        }
    }
}
