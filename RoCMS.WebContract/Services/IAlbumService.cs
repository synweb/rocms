using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IAlbumService
    {
        void AddImageToAlbum(int albumId, string imageId);

        int CreateAlbum(string name, int? ownerId);
        
        void RemoveImageFromAlbum(int albumId, string imageId);

        void RemoveAlbum(int albumId);

        

        string GetRandomImageFromAlbum(int albumId);

        Album GetAlbum(int albumId);
        ICollection<Album> GetUserAlbums(int? ownerId = null);
        ICollection<Album> GetAlbums();
        ICollection<Album> GetSystemAlbums();
       

        IEnumerable<AlbumImageInfo> GetAlbumImages(int albumId);

        void UpdateImageTitle(int albumId, string imageId, string title);

        void UpdateImageDescription(int albumId, string imageId, string description);

        void UpdateImagesSortOrder(int albumId, IList<string> imageIds);

        void UpdateAlbum(Album album);
        void UpdateImageDestinationUrl(int albumId, string imageId, string destinationUrl);
        
    }
}
