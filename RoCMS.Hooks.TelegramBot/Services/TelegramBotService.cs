using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RoCMS.Hooks.TelegramBot.Services
{
    public class TelegramBotService: ITelegramBotService
    {
        private TelegramBotClient bot;


        public void StartBot()
        {
            var key = "762376791:AAHN88a4uP1q-tboZE5AOCXg-b-O8Bjr56M";
            bot = new Telegram.Bot.TelegramBotClient(key);
            //await bot.SendTextMessageAsync()
        }

        public async void ReceiveMessage()
        {
            var updates = await bot.GetUpdatesAsync(0);
            foreach (var update in updates) // Перебираем все обновления
            {
                var message = update.Message;
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (message.Text == "/saysomething")
                    {
                        // в ответ на команду /saysomething выводим сообщение
                        await bot.SendTextMessageAsync(message.Chat.Id, "тест",
                            replyToMessageId: message.MessageId);
                    }
                }

            }
        }
    }
}
