using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base.Infrastructure;
using RoCMS.Web.Contract.Services;
using RoCMS.Hooks.TelegramBot.Services;
using Telegram.Bot;

namespace RoCMS.Hooks.TelegramBot
{
    public class TelegramBotHookModuleInitializer : IModuleInitializer
    {

        public void Init()
        {

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            var telegramBotService = DependencyResolver.Current.GetService<ITelegramBotService>();
            telegramBotService.StartBot();
        }

        private async void FormRequestService_RequestCreated(object sender, Web.Contract.Models.FormRequest e)
        {
            var settingsService = DependencyResolver.Current.GetService<ISettingsService>();


        }
    }
}
