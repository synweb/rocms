using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Hooks.TelegramBot.Services
{
    public interface ITelegramBotService
    {
        void StartBot();
        void ReceiveMessage();
    }
}
