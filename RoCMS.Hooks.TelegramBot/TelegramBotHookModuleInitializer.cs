using System.Net.Http;
using System.Text;
using System.Web.Http;
using RoCMS.Base.Infrastructure;
using RoCMS.Web.Contract.Services;
using System.Web.Mvc;
using System.Web.Routing;
using RoCMS.Hooks.TelegramBot.Services;

namespace RoCMS.Hooks.TelegramBot
{
    public class TelegramBotHookModuleInitializer : IModuleInitializer
    {

        public void Init()
        {
            var formRequestService = DependencyResolver.Current.GetService<IFormRequestService>();
            formRequestService.RequestCreated += FormRequestService_RequestCreated;


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
