using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using RoCMS.Base.Exceptions;
using RoCMS.Base.Helpers;
using RoCMS.Data;
using RoCMS.Data.Gateways;
using RoCMS.Data.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class PasswordTicketService: BaseCoreService, IPasswordTicketService
    {
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly IMailService _mailService;

        private readonly PasswordTicketGateway _passwordTicketGateway = new PasswordTicketGateway();

        public PasswordTicketService(ISettingsService settingsService, ISecurityService securityService, IMailService mailService)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _mailService = mailService;
        }

        public Guid CreateTicket(string email)
        {
            var user = _securityService.GetUserByEmail(email);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            DateTime utcNow = DateTime.UtcNow;
            TimeSpan lifetime = _settingsService.GetTicketLifetime();
            Guid token = Guid.NewGuid();

            var ticket = new PasswordTicket()
            {
                ExpirationDate = utcNow.Add(lifetime),
                Token = token,
                UserId = user.UserId,
            };
            _passwordTicketGateway.Insert(ticket);
            var url = _settingsService.GetSettings<string>("RootUrl");
            string link = String.Format("{0}/restorepassword/{1}", url, token);
            var text = String.Format(
                "Вы запросили восстановление пароля на сайте {0} <br/>Ваш логин: {2}.<br/>Чтобы восстановить пароль, перейдите по ссылке {1} <br/><br/>Если Вы не запрашивали восстановление пароля, пожалуйста, проигнорируйте это письмо.<br/><br/>С уважением, администрация сайта {0}.",
                url, link, user.Username);
            
            _mailService.Send(new MailMsg()
            {
                Receiver = email,
                Subject = String.Format("Восстановление пароля на сайте {0}", url),
                Body = text
            });

            return token;
        }

        public void UseTicket(Guid token, string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("password");
            }
            var ticket = _passwordTicketGateway.SelectAvailableByToken(token);
            if (ticket == null)
            {
                throw new ObjectNotFoundException("Неверный токен восстановления");
            }
            using (var transaction = new TransactionScope())
            {
                _securityService.SetPassword(ticket.UserId, password);
                _passwordTicketGateway.UseTicket(ticket.TicketId);
                transaction.Complete();
            }
        }
    }
}
