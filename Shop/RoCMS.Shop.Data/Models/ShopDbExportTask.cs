using System;

namespace RoCMS.Shop.Data.Models
{
    public class ShopDbExportTask
    {
        public int TaskId { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
    }
}
