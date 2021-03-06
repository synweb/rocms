﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MihaZupan;
using RoCMS.Base.Helpers;
using RoCMS.Hooks.TelegramBot.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Message = Telegram.Bot.Types.Message;

namespace RoCMS.Hooks.TelegramBot.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private TelegramBotClient _botClient;
        private readonly ILogService _logService;
        private readonly IFormRequestService _formRequestService;
        private ISettingsService _settingsService;

        private readonly List<PhoneChatId> _phoneChatIds;
        private List<string> _allowedPhoneNumbers = new List<string>();

        private readonly string _phoneChatIdsFile;

        public TelegramBotService(ILogService logService, IFormRequestService formRequestService1, ISettingsService settingsService)
        {
            _logService = logService;
            _formRequestService = formRequestService1;
            _settingsService = settingsService;

            _formRequestService.RequestCreated += FormRequestService_RequestCreated;

            _phoneChatIds = new List<PhoneChatId>();

            _phoneChatIdsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "phonechatids.xml");

            if (!File.Exists(_phoneChatIdsFile))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<PhoneChatId>));
                using (StreamWriter tw = new StreamWriter(_phoneChatIdsFile))
                {
                    xs.Serialize(tw, _phoneChatIds);
                }
            }


            XmlSerializer xsr = new XmlSerializer(typeof(List<PhoneChatId>));
            using (StreamReader tw = new StreamReader(_phoneChatIdsFile))
            {
                _phoneChatIds = (List<PhoneChatId>)xsr.Deserialize(tw);
            }

            var allowedUserPhones = _settingsService.GetSettings<string>("Hooks_TelegramBot_AllowedUserPhones");

            if (!String.IsNullOrEmpty(allowedUserPhones))
            {
                _allowedPhoneNumbers = allowedUserPhones.Split(',').ToList();
                foreach (var phoneChatId in _phoneChatIds)
                {
                    phoneChatId.Allowed = _allowedPhoneNumbers.Any(x => x == phoneChatId.Phone);
                }
            }

        }

        public void StartBot()
        {
            try
            {
                string proxyServer = _settingsService.GetSettings<string>("Hooks_TelegramBot_ProxyServer");
                int proxyPort = _settingsService.GetSettings<int>("Hooks_TelegramBot_ProxyPort");
                string proxyLogin = _settingsService.GetSettings<string>("Hooks_TelegramBot_ProxyLogin");
                string proxyPassword = _settingsService.GetSettings<string>("Hooks_TelegramBot_ProxyPassword");

                string key = _settingsService.GetSettings<string>("Hooks_TelegramBot_ApiKey");


                var proxy = new HttpToSocks5Proxy(
                    proxyServer, proxyPort, proxyLogin, proxyPassword
                );

                proxy.ResolveHostnamesLocally = false;

                _botClient = new TelegramBotClient(key, proxy);

                var me = _botClient.GetMeAsync().Result;

                _logService.TraceMessage(
                    $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
                );

                _botClient.OnMessage += Bot_OnMessage;
                _botClient.StartReceiving();
            }
            catch (Exception e)
            {
                _logService.LogError(e);
            }
        }

        public void ReceiveMessage(Update update)
        {
            try
            {
                Receive(update.Message);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
            }
        }

        public void UpdateSettings(TelegramBotSettings settings)
        {
            _settingsService.Set("Hooks_TelegramBot_ProxyServer", settings.ProxyServer);
            _settingsService.Set("Hooks_TelegramBot_ProxyPort", settings.ProxyPort);
            _settingsService.Set("Hooks_TelegramBot_ProxyLogin", settings.ProxyLogin);
            _settingsService.Set("Hooks_TelegramBot_ProxyPassword", settings.ProxyPassword);
            _settingsService.Set("Hooks_TelegramBot_AllowedUserPhones", settings.AllowedUserPhones);
            _settingsService.Set("Hooks_TelegramBot_ApiKey", settings.ApiKey);
            _settingsService.Set("Hooks_TelegramBot_WebHookToken", settings.WebHookToken);


            _allowedPhoneNumbers = settings.AllowedUserPhones.Split(',').ToList();
            foreach (var phoneChatId in _phoneChatIds)
            {
                phoneChatId.Allowed = _allowedPhoneNumbers.Any(x => x == phoneChatId.Phone);
            }
        }

        public TelegramBotSettings GetSettings()
        {

            string proxyServer = _settingsService.GetSettings<string>("Hooks_TelegramBot_ProxyServer");
            int proxyPort = _settingsService.GetSettings<int>("Hooks_TelegramBot_ProxyPort");
            string proxyLogin = _settingsService.GetSettings<string>("Hooks_TelegramBot_ProxyLogin");
            string proxyPassword = _settingsService.GetSettings<string>("Hooks_TelegramBot_ProxyPassword");
            string allowedUserNames = _settingsService.GetSettings<string>("Hooks_TelegramBot_AllowedUserPhones");
            string key = _settingsService.GetSettings<string>("Hooks_TelegramBot_ApiKey");
            string token = _settingsService.GetSettings<string>("Hooks_TelegramBot_WebHookToken");

            TelegramBotSettings settings = new TelegramBotSettings()
            {
                AllowedUserPhones = allowedUserNames,
                ApiKey = key,
                ProxyLogin = proxyLogin,
                ProxyPassword = proxyPassword,
                ProxyPort = proxyPort,
                ProxyServer = proxyServer,
                WebHookToken = token
            };

            return settings;

        }

        private void FormRequestService_RequestCreated(object sender, FormRequest e)
        {
            try
            {
                if (e != null && _botClient != null && _phoneChatIds.Any(x => x.Allowed))
                {
                    string message = $"Лид {e.CreationDate.ToShortDateString()} {e.CreationDate.ToShortTimeString()}\n\n{e.Text}";
                    foreach (var id in _phoneChatIds.Where(x => x.Allowed))
                    {
                        _botClient.SendTextMessageAsync(
                            chatId: id.ChatId,
                            text: message
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(ex);
            }
        }

        async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                await Receive(e.Message);
            }
            catch (Exception ex)
            {
                _logService.LogError(ex);
            }
        }

        private async Task Receive(Message message)
        {
            try
            {
                var phoneChatId = _phoneChatIds.FirstOrDefault(x => x.ChatId == message.Chat.Id);
                if (phoneChatId == null && message.Contact == null)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: message.Chat,
                        text: "Пожалуйста, расшарьте Ваш номер телефона",
                        replyMarkup: new ReplyKeyboardMarkup(
                            new[] { KeyboardButton.WithRequestContact("Share Contact") }, true, true
                        )
                    );
                    _logService.TraceMessage($"Новый пользователь: {message.Chat.Username}");
                }
                else
                {
                    if (phoneChatId == null)
                    {
                        lock (this)
                        {
                            string phone = message.Contact.PhoneNumber.Replace("+", "");
                            phoneChatId = new PhoneChatId()
                            {
                                ChatId = message.Chat.Id,
                                Phone = phone,
                                Allowed = _allowedPhoneNumbers.Any(x => x == phone)
                            };
                            _phoneChatIds.Add(phoneChatId);

                            XmlSerializer xsr = new XmlSerializer(typeof(List<PhoneChatId>));
                            using (StreamWriter tw = new StreamWriter(_phoneChatIdsFile))
                            {
                                xsr.Serialize(tw, _phoneChatIds);
                            }
                        }
                    }
                    if (phoneChatId.Allowed)
                    {
                        await _botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Вы подключены. Заявки будут высылаться вам автоматически."
                        );
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "У вас нет доступа"
                        );
                    }
                }
            }
            catch (Exception e)
            {
                _logService.LogError(e);
            }
        }

    }
}
