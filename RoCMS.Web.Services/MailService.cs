using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.UI;
using RoCMS.Base.Helpers;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using RoCMS.Web.Contract;
using AutoMapper;
using RoCMS.Data.Gateways;
using System.Collections.Generic;

namespace RoCMS.Web.Services
{
    public class MailService : BaseCoreService, IMailService
    {
        private readonly ITempFilesService _tempFilesService;
        private readonly IRazorEngineService _razorEngineService;
        private readonly ILogService _logService;
        private readonly ISettingsService _settingsService;
        private readonly MailGateway _mailGateway = new MailGateway();

        public MailService(ITempFilesService tempFilesService, IRazorEngineService razorEngineService, ILogService logService, ISettingsService settingsService)
        {
            _tempFilesService = tempFilesService;
            _razorEngineService = razorEngineService;
            _logService = logService;
            _settingsService = settingsService;

            Mapper.CreateMap<RoCMS.Web.Contract.Models.MailMsg, RoCMS.Data.Models.Mail>()
                 .ForMember(x => x.MailId, x => x.Ignore())
                 .ForMember(x => x.CreationDate, x => x.Ignore())
                 .ForMember(x => x.ErrorMessage, x => x.Ignore())
                 .ForMember(x => x.Sent, x => x.Ignore())
                 .ForMember(x => x.Attaches, x => x.ResolveUsing(s =>
                 {
                     string result = "";
                     var attachIds = s.AttachIds;
                     if (attachIds != null && attachIds.Any())
                     {
                         foreach (Guid id in attachIds)
                         {
                             try
                             {
                                 var file = _tempFilesService.GetFile(id);
                                 if (file != null)
                                 {
                                     if (result.Length > 0)
                                     {
                                         result += ",";
                                     }
                                     result += file.FileName;
                                 }
                             }
                             catch (Exception e)
                             {
                                 _logService.LogError(e);
                             }
                         }
                     }
                     return result;
                 }));
            Mapper.AssertConfigurationIsValid();
        }

        public void Send(MailMsg msg)
        {
            return;
            var mail = Mapper.Map<RoCMS.Data.Models.Mail>(msg);
            using (MailMessage mailMessage = MailMsgToMailMessage(msg))
            {
                mail.MailId = _mailGateway.Insert(mail);
                MailSendResult sendResult = Send(mailMessage);
                mail.Sent = sendResult.Success;
                mail.ErrorMessage = sendResult.ErrorMsg;
                _mailGateway.Update(mail);
            }
        }

        private MailMessage MailMsgToMailMessage(MailMsg msg)
        {
            return new MailMessage();
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_settingsService.GetSettings<string>("SystemEmailAddress"), _settingsService.GetSettings<string>("SystemEmailSenderName"));
            var receivers = msg.Receiver.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string receiver in receivers)
            {
                mailMessage.To.Add(new MailAddress(receiver));
            }
            mailMessage.Subject = msg.Subject;
            mailMessage.IsBodyHtml = true;
            string body = _razorEngineService.RenderEmailMessage("TextMessage", msg.Body);
            mailMessage.Body = body;
            if (!String.IsNullOrWhiteSpace(msg.BccReceiver))
            {
                mailMessage.Bcc.Add(new MailAddress(msg.BccReceiver));
            }
            if (msg.AttachIds != null && msg.AttachIds.Any())
            {
                AttachFiles(mailMessage, msg.AttachIds);
            }
            return mailMessage;
        }

        private MailSendResult Send(MailMessage message)
        {
            return new MailSendResult();
            MailSendResult sendResult = new MailSendResult();

            if (message.From == null || String.IsNullOrEmpty(message.From.Address))
            {
                message.From = new MailAddress(_settingsService.GetSettings<string>("SystemEmailAddress"),
                    _settingsService.GetSettings<string>("SystemEmailSenderName"));
            }
            if (AppSettingsHelper.EmailBlindCarbonCopyEnabled)
            {
                string bcc = AppSettingsHelper.EmailBlindCarbonCopyAddress;
                message.Bcc.Add(new MailAddress(bcc));
            }
            //Авторизация на SMTP сервере
            using (SmtpClient smtp = new SmtpClient(_settingsService.GetSettings<string>("EmailSmtpUrl"), _settingsService.GetSettings<int>("EmailSmtpPort")))
            {
                smtp.Credentials = new NetworkCredential(_settingsService.GetSettings<string>("EmailLogin"), _settingsService.GetSettings<string>("EmailPassword"));
                smtp.EnableSsl = _settingsService.GetSettings<bool>("SmtpSslEnabled");

                try
                {
                    smtp.Send(message); //отправка
                    sendResult.Success = true;
                }
                catch (Exception e)
                {
                    _logService.LogError(e);
                    sendResult.Success = false;
                    sendResult.ErrorMsg = e.Message;
                }
            }
            return sendResult;
        }

        private void AttachFiles(MailMessage mailMessage, Guid[] attachIds)
        {
            if (attachIds != null && attachIds.Any())
            {
                foreach (Guid id in attachIds)
                {
                    try
                    {
                        var file = _tempFilesService.GetFile(id);
                        if (file != null)
                        {
                            mailMessage.Attachments.Add(new Attachment(new MemoryStream(file.Content),
                                file.FileName, file.MimeType));
                            _tempFilesService.RemoveFile(id);
                        }
                    }
                    catch (Exception e)
                    {
                        _logService.LogError(e);
                    }
                }
            }
        }

        public IList<Mail> GetMessages()
        {
            var list = _mailGateway.Select();
            return Mapper.Map<IList<Mail>>(list);
        }

        public void DeleteMessage(int mailId)
        {
            _mailGateway.Delete(mailId);
        }

        public MailSendResult ReSendMessage(int mailId)
        {
            var mail = _mailGateway.SelectOne(mailId);
            using (MailMessage mailMessage = MailMsgToMailMessage(Mapper.Map<MailMsg>(mail)))
            {
                MailSendResult sendResult = Send(mailMessage);
                mail.Sent = sendResult.Success;
                mail.ErrorMessage = sendResult.ErrorMsg;
                _mailGateway.Update(mail);
                return sendResult;
            }
        }
    }
}

