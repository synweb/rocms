using RoCMS.Base.Models;

namespace RoCMS.Web.Contract.Models
{
    public class VideoAlbum: IdNamePair<int>
    {
        public VideoAlbum()
        {
            
        }

        public VideoAlbum(int id, string name):base(id,name)
        {
            
        }
    }
}
