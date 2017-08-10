using RoCMS.Base.Helpers;
using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;
using RoCMS.Data.Gateways;
using AutoMapper;
using Newtonsoft.Json;
using RoCMS.Base.Services;
using RoCMS.Web.Contract.Extensions;

namespace RoCMS.Web.Services
{
    class FormRequestService : BaseCoreService, IFormRequestService
    {
        private FormRequestGateway _formRequestGateway;
        private readonly IMailService _mailService;
        private readonly ISettingsService _settingsService;
        private readonly IOrderFormService _orderFormService;

        public FormRequestService(IMailService mailService, ISettingsService settingsService, IOrderFormService orderFormService)
        {
            _formRequestGateway = new FormRequestGateway();
            _mailService = mailService;
            _settingsService = settingsService;
            _orderFormService = orderFormService;
        }
        protected override int CacheExpirationInMinutes
        {
            get
            {
                return AppSettingsHelper.HoursToExpireCartCache * 60;
            }
        }

        public int CreateFormRequest(RoCMS.Web.Contract.Models.FormRequest formRequest)
        {
            return _formRequestGateway.Insert(Mapper.Map<RoCMS.Data.Models.FormRequest>(formRequest));
        }

        public void DeleteFormRequest(int formRequestId)
        {
            _formRequestGateway.Delete(formRequestId);
        }

        public RoCMS.Web.Contract.Models.FormRequest GetOneFormRequest(int formRequestId)
        {
            return Mapper.Map<RoCMS.Web.Contract.Models.FormRequest>(_formRequestGateway.SelectOne(formRequestId));
        }

        public IList<RoCMS.Web.Contract.Models.FormRequest> GetFormRequests()
        {
            var list = _formRequestGateway.Select();
            return Mapper.Map<IList<RoCMS.Web.Contract.Models.FormRequest>>(list);
        }

        public void UpdateFormRequest(RoCMS.Web.Contract.Models.FormRequest formRequest)
        {
            _formRequestGateway.Update(Mapper.Map<RoCMS.Data.Models.FormRequest>(formRequest));
        }

        public void ProcessMessage(Message message)
        {
            if (message.OrderFormId.HasValue)
            {
                var orderForm = _orderFormService.GetOrderForm(message.OrderFormId.Value);
                Dictionary<string, string> values = message.Fields;
                StringBuilder messageText = new StringBuilder();

                int i = 0;
                foreach (var value in values)
                {
                    int fieldId;
                    if (Int32.TryParse(value.Key.Replace("fld", ""), out fieldId))
                    {
                        var field = orderForm.Fields.SingleOrDefault(x => x.OrderFormFieldId == fieldId);
                        if (field != null)
                        {
                            messageText.Append($"{field.LabelText}: ");
                        }
                    }
                    i++;

                    messageText.Append(value.Value);
                    messageText.AppendLine();
                }

                message.Text = messageText.ToString();
            }


            CreateFormRequest(Mapper.Map<RoCMS.Web.Contract.Models.FormRequest>(message));

            var msg = CreateMessage(message);
            _mailService.Send(msg);
            if (_settingsService.GetSettings<bool>("AutoEmailReplyEnabled") && !string.IsNullOrWhiteSpace(message.Email))
            {
                SendAutoReply(message.Email, message.Name, msg.Body);
            }
        }

        private void SendAutoReply(string email, string name, string message)
        {
            string template = _settingsService.GetSettings<string>("MailTmplOrderAutoReply");
            string body = string.Format(template, name, message);
            MailMsg reply = new MailMsg()
            {
                Subject = "Ваша заявка принята",
                Receiver = email,
                Body = body
            };
            _mailService.Send(reply);
        }

        private MailMsg CreateMessage(Message message)
        {
            //Формирование письма
            {
                string subject = String.Empty;
                string templateName = String.Empty;
                string template = String.Empty;
                string body = String.Empty;
                var rootUrl = _settingsService.GetSettings<string>(nameof(Setting.RootUrl))
                    .Replace("http://", "").Replace("https://", "");
                MailMsg res;
                if (message.OrderFormId.HasValue)
                {
                    var form = _orderFormService.GetOrderForm(message.OrderFormId.Value);
                    subject = String.IsNullOrEmpty(form.EmailSubject) ? $"Заявка с сайта {rootUrl}" : form.EmailSubject;
                    if (form.DateInEmailSubject)
                    {
                        subject = $"{subject}: {DateTime.UtcNow.ApplySiteTimezone()}";

                    }
                    body = String.IsNullOrWhiteSpace(form.EmailTemplate)
                        ? message.Text
                        : string.Format(form.EmailTemplate, message.Text);
                    res = new MailMsg()
                    {
                        Subject = subject,
                        Receiver = String.IsNullOrEmpty(form.Email) ? _settingsService.GetSettings<string>("OrderEmailAddress") : form.Email,
                        AttachIds = message.AttachIds,
                        Body = body.Replace("\r\n", "<br/><br/>").Replace("\n", "<br/><br/>"),
                        BccReceiver = form.BccEmail
                    };
                }
                else
                {
                    switch (message.MessageType)
                    {
                        case MessageType.CallMeBack:
                            subject = "Обратный звонок";
                            templateName = "MailTmplCallback";
                            template = _settingsService.GetSettings<string>(templateName);
                            body = string.Format(template, message.Name, message.Phone, message.Text);
                            break;
                        case MessageType.Order:
                            subject = $"Заявка с сайта {rootUrl}";
                            templateName = "MailTmplOrder";
                            template = _settingsService.GetSettings<string>(templateName);
                            body = string.Format(template, message.Name, message.Email, message.Phone, message.Contact,
                                message.Text);
                            break;
                    }

                    res = new MailMsg()
                    {
                        Subject = subject + ": " + DateTime.UtcNow.ApplySiteTimezone(),
                        Receiver = _settingsService.GetSettings<string>("OrderEmailAddress"),
                        AttachIds = message.AttachIds,
                        Body = body
                    };
                }
                return res;
            }
        }
    }
}
