using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using RoCMS.Base.Helpers;
using AutoMapper;
using System.Net.Http;
using System.Web.Script.Serialization;
using RoCMS.Data.Gateways;
using VideoAlbum = RoCMS.Data.Models.VideoAlbum;
using VideoInfo = RoCMS.Data.Models.VideoInfo;

namespace RoCMS.Web.Services
{
    class VideoGalleryService : BaseCoreService, IVideoGalleryService
    {
        private readonly ILogService _logService;
        private readonly ISettingsService _settingsService;

        private VideoAlbumGateway videoAlbumGateway;
        private VideoGateway _videoGateway;

        public VideoGalleryService(ILogService logService, ISettingsService settingsService)
        {
            _logService = logService;
            _settingsService = settingsService;
            videoAlbumGateway = new VideoAlbumGateway();
            _videoGateway = new VideoGateway();
        }        

        protected override int CacheExpirationInMinutes
        {
            get
            {
                return AppSettingsHelper.HoursToExpireCartCache * 60;
            }
        }

        #region YouTube classes
        class YoutubeResponse
        {
            public ICollection<YoutubeItem> items { get; set; }
        }

        class YoutubeItem
        {
            public string id { get; set; }
            public YoutubeSnippet snippet { get; set; }
            public YoutubeStats statistics { get; set; }
        }

        class YoutubeSnippet
        {
            public string channelId { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string categoryId { get; set; }
        }

        class YoutubeStats
        {
            public long viewCount { get; set; }
            public int likeCount { get; set; }
            public int dislikeCount { get; set; }
            public int favoriteCount { get; set; }
            public int commentCount { get; set; }
        }
        #endregion



        #region IVideoGalleryService
        public void AddVideo(int albumId, string videoId)
        {
            var apiKey = _settingsService.GetSettings().YoutubeAPIKey;
            YoutubeResponse response;
            using (var c = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage = c.GetAsync(
                    $"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={apiKey}&fields=items(id,snippet(channelId,title,categoryId,description),statistics)&part=snippet,statistics"
                    ).Result;
                string responseStr = httpResponseMessage.Content.ReadAsStringAsync().Result;
                _logService.TraceMessage($"Response from YouTube: {responseStr}");
                var serializer = new JavaScriptSerializer();
                response = serializer.Deserialize<YoutubeResponse>(responseStr);
            }
            var video = new VideoInfo()
            {
                VideoId = videoId,
                AlbumId = albumId,
                CreationDate = DateTime.UtcNow,
                Title = response.items.First().snippet.title,
                Description = response.items.First().snippet.description
            };
            _videoGateway.Insert(video);
        }

        public void CreateVideoAlbum(string title)
        {
            videoAlbumGateway.Insert( new VideoAlbum() {Name = title, CreationDate = DateTime.UtcNow});
        }

        public IEnumerable<Contract.Models.VideoInfo> GetAlbumVideos(int albumId)
        {
            var videos = _videoGateway.SelectByAlbum(albumId).OrderBy(x => x.SortOrder).ThenByDescending(x => x.CreationDate);
            var list = Mapper.Map<ICollection<RoCMS.Web.Contract.Models.VideoInfo>>(videos);
            return list;
        }

        public Contract.Models.VideoInfo GetVideo(string videoId)
        {
            var video = _videoGateway.SelectOne(videoId);
            return Mapper.Map<RoCMS.Web.Contract.Models.VideoInfo>(video);
        }

        public IEnumerable<Contract.Models.VideoAlbum> GetVideoAlbums()
        {
            var videoAlbums = videoAlbumGateway.Select();
            var list = Mapper.Map<ICollection<RoCMS.Web.Contract.Models.VideoAlbum>>(videoAlbums);
            return list;
        }

        public string GetVideoAlbumTitle(int albumId)
        {
            return videoAlbumGateway.SelectOne(albumId).Name;
        }

        public void RemoveVideo(string videoId)
        {
            _videoGateway.Delete(videoId);
        }

        public void RemoveVideoAlbum(int albumId)
        {
            videoAlbumGateway.Delete(albumId);
        }

        public void UpdateVideoAlbumTitle(int albumId, string title)
        {
            var videoAlbum = videoAlbumGateway.SelectOne(albumId);
            videoAlbum.Name = title;
            videoAlbumGateway.Update(videoAlbum);
        }

        public void UpdateVideoDescription(string videoId, string description)
        {
            var video = _videoGateway.SelectOne(videoId);
            video.Description = description;
            _videoGateway.Update(video);
        }

        public void UpdateVideosSortOrder(int albumId, IList<string> videoIds)
        {
            for (int i = 0; i < videoIds.Count; i++)
            {           
                var video = _videoGateway.SelectOne(videoIds[i]);
                video.VideoId = videoIds[i];
                video.SortOrder = i;
                _videoGateway.Update(video);               
            }
        }

        public void UpdateVideoTitle(string videoId, string title)
        {
            var video = _videoGateway.SelectOne(videoId);
            video.Title = title;
            _videoGateway.Update(video);
        }
        #endregion
    }
}
