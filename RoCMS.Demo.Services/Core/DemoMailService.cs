using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoMailService: IMailService
    {
        public void Send(MailMsg msg)
        {
            // запрещаем отправку сообщений
        }

        public IList<Mail> GetMessages()
        {
            return new List<Mail>();
        }

        public void DeleteMessage(int mailId)
        {
            // если сообщений нет, то и удалять нечего
        }

        public MailSendResult ReSendMessage(int mailId)
        {
            return new MailSendResult() {Success = false, ErrorMsg = "Emails are not supported in demo"};
        }
    }
}
