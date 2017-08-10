using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Shop.Contract.Models
{
    public enum DbExportStatus
    {
        Processing,
        Success,
        Error,
        CompleteWithErrors
    }
}
