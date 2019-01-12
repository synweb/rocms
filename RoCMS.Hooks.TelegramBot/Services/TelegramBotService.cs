using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MihaZupan;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RoCMS.Hooks.TelegramBot.Services
{
    public class TelegramBotService: ITelegramBotService
    {
        private TelegramBotClient botClient;
        private readonly ILogService _logService;
        private IFormRequestService _formRequestService;

        private List<ChatId> _chatIds;


        public TelegramBotService(ILogService logService, IFormRequestService formRequestService1)
        {
            _logService = logService;
            _formRequestService = formRequestService1;


            _formRequestService.RequestCreated += FormRequestService_RequestCreated;

            
            _chatIds = new List<ChatId>(); 
        }

        public void StartBot()
        {

            var proxy = new HttpToSocks5Proxy(
                "91.235.137.217", 1080, "teleproxy", "W57zC8CBUhCcEVTK"
            );

            proxy.ResolveHostnamesLocally = true;

            var key = "762376791:AAHN88a4uP1q-tboZE5AOCXg-b-O8Bjr56M";

            botClient = new TelegramBotClient(key, proxy);
            var me = botClient.GetMeAsync().Result;

            _logService.TraceMessage(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

        }

        public async void ReceiveMessage()
        {
            var updates = await botClient.GetUpdatesAsync(0);
            foreach (var update in updates) // Перебираем все обновления
            {
                var message = update.Message;
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    _logService.TraceMessage("From bot: " + message.Text);
                    //if (message.Text == "/saysomething")
                    {
                        // в ответ на команду /saysomething выводим сообщение
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Получено: {message.Text}",
                            replyToMessageId: message.MessageId);
                    }
                }

            }
        }

        private void FormRequestService_RequestCreated(object sender, FormRequest e)
        {
            if (e != null && botClient != null && _chatIds.Any())
            {

                string message = $"Лид {e.CreationDate.ToShortDateString()} {e.CreationDate.ToShortTimeString()}\n\n{e.Text}";
                foreach (var id in _chatIds)
                {
                    botClient.SendTextMessageAsync(
                        chatId: id,
                        text: message
                    );
                }
            }
        }

        async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (_chatIds.All(x => x.Identifier != e.Message.Chat.Id))
            {
                _chatIds.Add(e.Message.Chat);
            }

            if (e.Message.Text != null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "You said:\n" + e.Message.Text
                );
            }
        }
    }
}
