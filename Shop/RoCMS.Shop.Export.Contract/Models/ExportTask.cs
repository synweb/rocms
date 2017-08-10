using System;

namespace RoCMS.Shop.Export.Contract.Models
{
    public class ExportTask
    {
        public int TaskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ExportStatus Status { get; set; }
        public string ErrorCode { get; set; }
    }
}
