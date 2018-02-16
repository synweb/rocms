using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using AngleSharp;
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
        private readonly INewsItemService _newsItemService;
        private readonly IAlbumService _albumService;
        private readonly IImageService _imageService;
        private readonly ISettingsService _settingsService;

        public RssCrawlingService(INewsItemService newsItemService, IAlbumService albumService, ISettingsService settingsService, IImageService imageService)
        {
            _newsItemService = newsItemService;
            _albumService = albumService;
            _settingsService = settingsService;
            _imageService = imageService;
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
            public async void Execute(IJobExecutionContext context)
            {
                var dataMap = context.JobDetail.JobDataMap;
                var feed = (RssCrawler) dataMap[FEED];
                var albumService = (IAlbumService) dataMap[ALBUM_SERVICE];
                var imageService = (IImageService) dataMap[IMAGE_SERVICE];
                var newsItemService = (INewsItemService) dataMap[NEWS_ITEM_SERVICE];
                var settingsService = (ISettingsService) dataMap[SETTINGS_SERVICE];

                XmlReader reader = XmlReader.Create(feed.RssFeedUrl);
                SyndicationFeed syndicationFeed = SyndicationFeed.Load(reader);
                foreach (SyndicationItem item in syndicationFeed.Items)
                {
                    if (!CheckIfItemIsNew(item, newsItemService))
                        continue;

                    // проверка текста по фильтрам
                    string title = item.Title.Text.Trim();
                    string textWithoutHtml = ParsingHelper.RemoveHtml(item.Summary.Text).Trim();
                    bool filterOk = true;
                    foreach (var filter in feed.Filters)
                    {
                        // фильтры реализованы по логическому "И"
                        // пройдут только те записи, которые соответствуют всем фильтрам
                        var titleMatches = Regex.IsMatch(title, filter.Filter);
                        var descriptionMatches = Regex.IsMatch(title, filter.Filter);
                        // если совпадение по регулярке найдено в заголовке или описании, считаем, что фильтр пройден
                        var match = (titleMatches || descriptionMatches);
                        filterOk &= match;
                        if (!match)
                            break;
                    }
                    if(!filterOk)
                        continue;

                    string imageId = null;
                    if (!string.IsNullOrEmpty(feed.ImageSelector))
                    {
                        // есть селектор для парсинга картинок
                        // Setup the configuration to support document loading
                        var config = Configuration.Default.WithDefaultLoader();
                        var address = item.Links.First().Uri;
                        // Asynchronously get the document in a new context using the configuration
                        var document = await BrowsingContext.New(config).OpenAsync(address.ToString());
                        // Perform the query to get all cells with the content
                        var cell = document.QuerySelector(feed.ImageSelector);
                        if (cell != null)
                        {
                            var url = cell.Attributes["src"].Value;
                            if (url.StartsWith("//"))
                            {
                                // ссылку вида //site.ru/img.jpg преобразуем в http://site.ru/img.jpg
                                url = $"http:{url}";
                            }
                            imageId = await albumService.DownloadImage(url);
                        }
                    }
                    lock (feed)
                    {
                        if (!CheckIfItemIsNew(item, newsItemService))
                        {
                            // если вдруг за время скачивания картинки другой поток успел добавить эту же новость
                            imageService.RemoveImage(imageId);
                            continue;
                        }
                            
                        const int NEWS_DESCRIPTION_LENGTH = 200;
                        var cuttedText = TextCutHelper.Cut(textWithoutHtml, NEWS_DESCRIPTION_LENGTH).Trim();
                        bool translitUrls = settingsService.GetSettings<bool>(nameof(Setting.TranslitEnabled));
                        var relativeUrl = translitUrls
                            ? FormattingHelper.ToTranslitedUrl(title)
                            : FormattingHelper.ToRussianURL(title);
                        var newNewsItem = new NewsItem()
                        {
                            RssSource = item.Id,
                            AuthorId = null,
                            BreadcrumbsTitle = title,
                            Text = item.Summary.Text.Trim(),
                            ImageId = imageId,
                            PostingDate = item.PublishDate.DateTime,
                            Description = cuttedText,
                            MetaDescription = cuttedText,
                            Title = title,
                            RecordType = RecordType.Default,
                            Layout = "clientLayout",
                            RelativeUrl = relativeUrl
                        };
                        if (feed.TargetCategoryId != null)
                        {
                            newNewsItem.Categories = new List<IdNamePair<int>>()
                            {
                                new IdNamePair<int>(feed.TargetCategoryId.Value, string.Empty)
                            };
                        }
                        newsItemService.CreateNewsItem(newNewsItem);
                    }
                }
            }

            private static bool CheckIfItemIsNew(SyndicationItem item, INewsItemService newsItemService)
            {
                // можно оптимизировать, вынимая новость по конкретному RssSource
                var news = newsItemService.GetAllNews();
                return news.All(x => !x.RssSource.Equals(item.Id));
            }
        }
    }
}
