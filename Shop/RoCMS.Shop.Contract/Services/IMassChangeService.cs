using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IMassChangeService
    {
        MassPriceChangeTask StartChangePriceTask(MassPriceChange change);
        IEnumerable<MassPriceChangeTask> GetChangePriceTasks();
    }
}
