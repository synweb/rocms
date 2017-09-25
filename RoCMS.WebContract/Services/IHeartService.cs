using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RoCMS.Web.Contract.Infrastructure;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IHeartService
    {
        string GetCanonicalUrl(string relativeUrl);
        string GetNextAvailableRelativeUrl(string relativeUrl);
        void DeleteHeart(int id);
        string GetCanonicalUrl(int id);
        void UpdateHeart(Heart heart);
        int CreateHeart(Heart heart);
        void Fill(Heart heart);
        Heart GetHeart(string relativeUrl);
        Heart GetHeart(int heartId);
        ICollection<UrlPair> GetHeartUrls(Type type);
        ICollection<Heart> GetHearts();

        ICollection<Heart> GetHearts(IEnumerable<int> heartIds);
        bool CheckIfUrlExists(string relativeUrl);

    }
}
