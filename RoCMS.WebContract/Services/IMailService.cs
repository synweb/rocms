using System;
using System.Net.Mail;
using RoCMS.Web.Contract.Models;
using System.Collections.Generic;

namespace RoCMS.Web.Contract.Services
{
    public interface IMailService
    {
        void Send(MailMsg msg);
        IList<Mail> GetMessages();
        void DeleteMessage(int mailId);
        MailSendResult ReSendMessage(int mailId);
    }
}
