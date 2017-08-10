using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Shop.Contract.Models
{
    public class DbExportTask
    {
        public int TaskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DbExportStatus Status { get; set; }
        public string ErrorCode { get; set; }
    }
}
