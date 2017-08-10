using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class ImageInAlbumGateway : BaseGateway
    {
        public ICollection<ImageInAlbum> SelectByAlbum(int albumId)
        {
            return ExecSelect<ImageInAlbum>(GetProcedureString(), albumId);
        }
        public int SelectCountByAlbum(int albumId)
        {
            return Exec<int>(GetProcedureString(), albumId);
        }

        public void Delete(int albumId, string imageId)
        {
            Exec(GetProcedureString(), new {albumId, imageId});
        }

        public void Insert(ImageInAlbum rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public ImageInAlbum SelectOne(int albumId, string imageId)
        {
            return Exec<ImageInAlbum>(GetProcedureString(), new {albumId, imageId});
        }

        public void Update(ImageInAlbum iia)
        {
            Exec(GetProcedureString(), iia);
        }

        public ICollection<ImageInAlbum> SelectByImage(string imageId)
        {
            return ExecSelect<ImageInAlbum>(GetProcedureString(), imageId); 
        }
    }
}
