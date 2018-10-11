using Newtonsoft.Json;
using RoCMS.Base.Infrastructure;
using RoCMS.Hooks.Bitrix24.External;
using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoCMS.Hooks.Bitrix24
{
    public class Bitrix24HookModuleInitializer : IModuleInitializer
    {
        // required settings:
        // Hooks_Bitrix24_ApiKey
        // Hooks_Bitrix24_UserId
        // Hooks_Bitrix24_InstanceName

        public void Init()
        {
            var formRequestService = DependencyResolver.Current.GetService<IFormRequestService>();
            formRequestService.RequestCreated += FormRequestService_RequestCreated;
        }

        private async void FormRequestService_RequestCreated(object sender, Web.Contract.Models.FormRequest e)
        {
            var settingsService = DependencyResolver.Current.GetService<ISettingsService>();
            const string URL_FORMAT = "https://{0}.bitrix24.ru/rest/{1}/{2}/{3}/";
            // {0}: userId
            // {1}: api key
            // {2}: method
            string bitrixApiKey = settingsService.GetSettings<string>("Hooks_Bitrix24_ApiKey");
            if (string.IsNullOrWhiteSpace(bitrixApiKey))
            {
                return;
            }
            int bitrixUserId = settingsService.GetSettings<int>("Hooks_Bitrix24_UserId");
            string instanceName = settingsService.GetSettings<string>("Hooks_Bitrix24_InstanceName");
            const string METHOD_LEAD_ADD = "crm.lead.add";
            var leadAddUrl = string.Format(URL_FORMAT, instanceName, bitrixUserId, bitrixApiKey, METHOD_LEAD_ADD);
            using (var client = new HttpClient())
            {
                var lead = new BitrixLead()
                {
                    Title = e.Name,
                    Name = e.Name,
                    SecondName = "(import)",
                    LastName = "(import)",
                    StatusId = "NEW",
                    Opened = "Y",
                    AssignedById = bitrixUserId,
                    CurrencyId = "RUR",
                    Opportunity = 0,
                    Comments = e.Text
                };
                if (!string.IsNullOrWhiteSpace(e.Phone))
                {
                    lead.Phone = new BitrixContact[] { new BitrixContact() {
                        Value = e.Phone,
                        ValueType = "WORK"
                    }};
                }
                if (!string.IsNullOrWhiteSpace(e.Email))
                {
                    lead.Email = new BitrixContact[] { new BitrixContact() {
                        Value = e.Email,
                        ValueType = "WORK"
                    }};
                }
                var model = new BitrixApiModel() { Fields = lead, Params = new { REGISTER_SONET_EVENT = "Y" } };
                var json = JsonConvert.SerializeObject(model);
                var response = await client.PostAsync(leadAddUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
