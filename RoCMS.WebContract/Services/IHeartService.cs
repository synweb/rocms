using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IHeartService
    {
        string GetCanonicalUrl(string relativeUrl);
        string GetNextAvailableRelativeUrl(string relativeUrl);
        void DeleteHeart(int id);
        void UpdateHeart(Heart heart);
        int CreateHeart(Heart heart);
        void Fill(Heart heart);
    }
}
