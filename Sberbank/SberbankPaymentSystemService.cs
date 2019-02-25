using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoCMS.SberbankPaymentSystem.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.SberbankPaymentSystem
{
    public class SberbankPaymentSystemService: IPaymentSystemService
    {
        private ISettingsService _settingsService;
        private ILogService _logService;

        private SberbankSettings _sberbankSettings;

        public SberbankPaymentSystemService(ISettingsService settingsService, ILogService logService)
        {
            _settingsService = settingsService;
            _logService = logService;
            FillSettings();


        }

        public string ProcessPayment(Guid orderId, decimal amount, string returnUrl)
        {


            RegisterPreAuthRequest req = new RegisterPreAuthRequest();
            req.orderNumber = orderId.ToString();
            req.returnUrl = returnUrl;
            req.amount = Decimal.ToInt32(Math.Round(amount * 100)); //копейки
            req.currency = 643;
            var result = RegisterPreAuth(req);

            return result.formUrl;

        }

        private void FillSettings()
        {
            _sberbankSettings = new SberbankSettings
            {
                Token = _settingsService.GetSettings<string>("Sberbank_Token"),
                BaseUrl = _settingsService.GetSettings<string>("Sberbank_BaseUrl")
            };
        }

        private RegisterPreAuthResponse RegisterPreAuth(RegisterPreAuthRequest registerParams)
        {
            RegisterPreAuthResponse res = null;
            try
            {
                // single-phase payment
                var url = _sberbankSettings.BaseUrl.Trim('/') + "/register.do";

                // payment with holding
                //var url = _sberbankSettings.BaseUrl.Trim('/') + "/registerPreAuth.do";
                

                //registerParams.token = _sberbankSettings.Token;
                registerParams.userName = "vorotmaster-api";
                registerParams.password = "vorotmaster";


                var urlParams = ObjectToQueryString(registerParams);
                url += "?" + urlParams;


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Proxy = null;
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.ServicePoint.ConnectionLimit = 1;
                request.Headers.Add("UserAgent", "Pentia; MSI");


                try
                {
                    HttpWebResponse getResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
                    {
                        string json = sr.ReadToEnd();
                        var result = (RegisterPreAuthResponse)JsonConvert.DeserializeObject<RegisterPreAuthResponse>(json);
                        
                        return result;
                    }

                }
                catch (Exception e)
                {
                    _logService.LogError(e);

                    throw;
                }


            }
            catch (Exception e)
            {
                _logService.LogError(e);
            }
            return res;
        }

        private string ObjectToQueryString(object obj)
        {
            string result = "";
            var properties = GetProperties(obj);

            foreach (var p in properties)
            {
                var value = p.GetValue(obj, null);
                var name = p.Name;
                if (value != null)
                {
                    result += $"{name}={value}&";
                }
            }
            return result;
        }

        private PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}
