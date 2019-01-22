using System;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Hooks.TelegramBot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var settingsService = DependencyResolver.Current.GetService<ISettingsService>();
            string token = settingsService.GetSettings<string>("Hooks_TelegramBot_WebHookToken");

            if (!String.IsNullOrWhiteSpace(token))
            {
                WebApiConfigHelper.ApiRoute(config.Routes, $"webhook/telegrambot/{token}/receive", "TelegramBotApi",
                    "ReceiveWebHook");
            }

            WebApiConfigHelper.ApiRoute(config.Routes, "telegrambot/settings/update", "TelegramBotSettingsApi", "Update");
            WebApiConfigHelper.ApiRoute(config.Routes, "telegrambot/settings/get", "TelegramBotSettingsApi", "Get");


        }
    }
}
