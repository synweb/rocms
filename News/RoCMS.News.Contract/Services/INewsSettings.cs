using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.News.Contract.Models;

namespace RoCMS.News.Contract.Services
{
    public interface INewsSettingsService
    {
        NewsSettings GetNewsSettings();
        void UpdateNewsSettings(NewsSettings settings);
    }


}
