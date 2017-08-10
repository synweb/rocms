using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IVideoGalleryService
    {
        void AddVideo(int albumId, string videoId);

        void CreateVideoAlbum(string title);

        void RemoveVideo(string videoId);

        void RemoveVideoAlbum(int albumId);

        IEnumerable<VideoAlbum> GetVideoAlbums();

        string GetVideoAlbumTitle(int albumId);

        IEnumerable<VideoInfo> GetAlbumVideos(int albumId);

        void UpdateVideoTitle(string videoId, string title);

        void UpdateVideoDescription(string videoId, string description);

        void UpdateVideosSortOrder(int albumId, IList<string> videoIds);

        void UpdateVideoAlbumTitle(int albumId, string title);
        VideoInfo GetVideo(string videoId);
    }
}
