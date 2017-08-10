using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopDbExportService
    {
        DbExportTask StartDbExportTask();
        List<DbExportTask> GetDbExportTasks(int count);
    }
}
