using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class VideoGateway : BaseGateway
    {
        public void Insert(VideoInfo video)
        {
            Exec("[dbo].[Video_Insert]", video);
        }

        public void Delete(string videoId)
        {
            Exec("[dbo].[Video_Delete]", videoId);
        }

        public VideoInfo SelectOne(string videoId)
        {
            return Exec<VideoInfo>("[dbo].[Video_SelectOne]", videoId);
        }

        public ICollection<VideoInfo> Select()
        {
            return ExecSelect<VideoInfo>("[dbo].[Video_Select]");
        }

        public ICollection<VideoInfo> SelectByAlbum(int albumId)
        {
            return ExecSelect<VideoInfo>("[dbo].[Video_SelectByAlbum]", albumId);
        }

        public void Update(VideoInfo video)
        {
            Exec("[dbo].[Video_Update]", video);
        }
    }
}
