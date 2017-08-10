using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Export.Contract.Models;

namespace RoCMS.Shop.Export.Contract
{
    public interface IExportShopService
    {
        ExportTask StartYmlExportTask(YmlExportSettings settings);
        List<ExportTask> GetYmlTasks(int count);
        string GetYmlFileContent();

        YmlExportSettings GetYmlExportSettings();

        void UpdateYmlExportSettings(YmlExportSettings settings);
    }
}
