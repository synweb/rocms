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
        IDictionary<string,string> GetHeartUrls(Type type);
        ICollection<Heart> GetHearts();
        ICollection<Heart> GetHearts(IEnumerable<int> heartIds);
        ICollection<Heart> GetHearts(string type);
        bool CheckIfUrlExists(string relativeUrl);
    }
}
