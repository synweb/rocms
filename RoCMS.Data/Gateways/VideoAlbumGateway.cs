using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class VideoAlbumGateway: BaseGateway
    {
        public int Insert(VideoAlbum album)
        {
           return Exec<int>("[dbo].[VideoAlbum_Insert]", album);
        }

        public void Delete(int albumId)
        {
            Exec("[dbo].[VideoAlbum_Delete]", albumId);
        }

        public VideoAlbum SelectOne(int albumId)
        {
            return Exec<VideoAlbum>("[dbo].[VideoAlbum_SelectOne]", albumId);
        }

        public ICollection<VideoAlbum> Select()
        {
            return ExecSelect<VideoAlbum>("[dbo].[VideoAlbum_Select]");
        }

        public void Update(VideoAlbum album)
        {
            Exec("[dbo].[VideoAlbum_Update]", album);
        }
    }
}
