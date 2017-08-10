using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.ViewModels
{
    public class AdminAlbumVM
    {
        public int AlbumId { get; set; }
        public IEnumerable<ImageInfo> AllImageInfos { get; set; }
        public IEnumerable<string> AlbumImageIds { get; set; }
        public string AlbumName { get; set; }
    }
}