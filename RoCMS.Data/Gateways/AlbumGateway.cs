using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class AlbumGateway: BasicGateway<Album>
    {
        public ICollection<Album> SelectUserAlbums(int? ownerId)
        {
            return ExecSelect<Album>(GetProcedureString(), ownerId);
        }
    }
}
