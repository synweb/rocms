using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Models
{
    public class OrderForm
    {
        public int OrderFormId { get; set; }
        public string EmailSubject { get; set; }

        public string Email { get; set; }
        public string BccEmail { get; set; }
        public string HtmlTemplate { get; set; }
        public string RedirectUrl { get; set; }
        public string SuccessMessage { get; set; }
        public string MetricsCode { get; set; }

        public bool DateInEmailSubject { get; set; }

        public bool FileAttachmentEnabled { get; set; }

        public int MaxFileAttachmentsCount { get; set; }

        public string Title { get; set; }

        public string EmailTemplate { get; set; }
        public string SendButtonText { get; set; }
        public string ClearButtonText { get; set; }
        public bool HideClearButton { get; set; }
    }
}
