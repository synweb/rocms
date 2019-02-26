using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    class FormRequestService : BaseCoreService, IFormRequestService
    {
        private readonly FormRequestGateway _formRequestGateway;
        private readonly IMailService _mailService;
        private readonly ISettingsService _settingsService;
        private readonly IOrderFormService _orderFormService;
        private readonly ILogService _logService;

        public event EventHandler<FormRequest> RequestCreated = (o, e) => { };

        public FormRequestService(IMailService mailService, ISettingsService settingsService, IOrderFormService orderFormService, ILogService logService)
        {
            _formRequestGateway = new FormRequestGateway();
            _mailService = mailService;
            _settingsService = settingsService;
            _orderFormService = orderFormService;
            _logService = logService;
        }
        protected override int CacheExpirationInMinutes => AppSettingsHelper.HoursToExpireCartCache * 60;

        public int CreateFormRequest(FormRequest formRequest)
        {
            var res = _formRequestGateway.Insert(Mapper.Map<Data.Models.FormRequest>(formRequest));
            formRequest.FormRequestId = res;
            try
            {
                RequestCreated(this, formRequest);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
            }
            return res;
        }

        public void UpdatePaymentState(int formRequestId, PaymentState? state, bool notify = false)
        {
            var req = _formRequestGateway.SelectOne(formRequestId);
            req.PaymentState = Mapper.Map<Data.Models.PaymentState>(state);
            _formRequestGateway.Update(req);

            if (notify)
            {



                string body = $"Поступила оплата за заказ<br>Guid: {req.Guid}<br>Сумма: {req.Amount}";
                MailMsg res = new MailMsg
                {
                    Subject = $"Заказ оплачен: {req.Guid} {DateTime.UtcNow.ApplySiteTimezone()}",
                    Receiver = _settingsService.GetSettings<string>("OrderEmailAddress"),

                    Body = body
                };
                _mailService.Send(res);







            }
        }

        public void UpdateFormRequestState(int formRequestId, FormRequestState state)
        {
            var req = _formRequestGateway.SelectOne(formRequestId);
            req.State = Mapper.Map<Data.Models.FormRequestState>(state);
            _formRequestGateway.Update(req);
        }

        public void DeleteFormRequest(int formRequestId)
        {
            _formRequestGateway.Delete(formRequestId);
        }

        public FormRequest GetOneFormRequest(int formRequestId)
        {
            return Mapper.Map<FormRequest>(_formRequestGateway.SelectOne(formRequestId));
        }

        public IList<FormRequest> GetFormRequests()
        {
            var list = _formRequestGateway.Select();
            return Mapper.Map<IList<FormRequest>>(list);
        }

        public void UpdateFormRequest(FormRequest formRequest)
        {
            _formRequestGateway.Update(Mapper.Map<Data.Models.FormRequest>(formRequest));
        }

        public int ProcessMessage(Message message)
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


            int id = CreateFormRequest(Mapper.Map<FormRequest>(message));

            var msg = CreateMessage(message);
            _mailService.Send(msg);
            if (_settingsService.GetSettings<bool>("AutoEmailReplyEnabled") && !string.IsNullOrWhiteSpace(message.Email))
            {
                SendAutoReply(message.Email, message.Name, msg.Body);
            }

            return id;
        }

        private void SendAutoReply(string email, string name, string message)
        {
            string template = _settingsService.GetSettings<string>("MailTmplOrderAutoReply");
            string body = string.Format(template, name, message);
            MailMsg reply = new MailMsg
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
                    res = new MailMsg
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

                    res = new MailMsg
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

        public FormRequest GetOneFormRequest(Guid guid)
        {
            return Mapper.Map<FormRequest>(_formRequestGateway.SelectByGuid(guid));
        }
    }
}
