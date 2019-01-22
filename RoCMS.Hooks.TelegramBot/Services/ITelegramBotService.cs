using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Hooks.TelegramBot.Models;
using Telegram.Bot.Types;

namespace RoCMS.Hooks.TelegramBot.Services
{
    public interface ITelegramBotService
    {
        void StartBot();
        void ReceiveMessage(Update model);
        void UpdateSettings(TelegramBotSettings settings);
        TelegramBotSettings GetSettings();
    }
}
