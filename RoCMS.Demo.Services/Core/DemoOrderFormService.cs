using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoOrderFormService: IOrderFormService
    {
        private List<OrderForm> _defaultOrderForms = new List<OrderForm>();

        public DemoOrderFormService()
        {
            FillDefaultData();
        }

        private void FillDefaultData()
        {
            try
            {
                var file = "orderforms.xml";
                var xs = new XmlSerializer(typeof(List<OrderForm>));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultOrderForms = (List<OrderForm>)xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultOrderForms.Add(new OrderForm()
                {
                    OrderFormId = 1,
                    Title = "Основная форма",
                    Email = "mail@rocms.ru",
                    BccEmail = "secret@rocms.ru",
                    DateInEmailSubject = true,
                    EmailSubject = "Новая заявка",
                    SuccessMessage = "Спасибо за заявку",
                    MaxFileAttachmentsCount = 1,
                    FileAttachmentEnabled = true,
                    HtmlTemplate = "{0}{1}",
                    EmailTemplate = "{0}{1}",
                    Fields = new List<OrderFormField>()
                    {
                        new OrderFormField()
                        {
                            OrderFormFieldId = 1,
                            OrderFormId = 1,
                            LabelText = "Имя",
                            Required = true,
                            SortOrder = 1,
                            ValueType = OrderFormFieldType.Text
                        },
                        new OrderFormField()
                        {
                            OrderFormFieldId = 2,
                            OrderFormId = 1,
                            LabelText = "Email",
                            Required = true,
                            SortOrder = 2,
                            ValueType = OrderFormFieldType.Email
                        },
                        new OrderFormField()
                        {
                            OrderFormFieldId = 3,
                            OrderFormId = 1,
                            LabelText = "Телефон",
                            Required = false,
                            SortOrder = 3,
                            ValueType = OrderFormFieldType.Phone
                        }
                    }
                });
            }
        }
        private const string ORDERFORMS_SESSION_KEY = "OrderForms";

        private void InitSessionDataIfEmpty(HttpContext ctx)
        {
            if (ctx.Session[ORDERFORMS_SESSION_KEY] == null)
            {
                ctx.Session[ORDERFORMS_SESSION_KEY] = _defaultOrderForms.ToList();
            }
        }

        private List<OrderForm> GetSessionForms(HttpContext ctx)
        {
            return (List<OrderForm>)ctx.Session[ORDERFORMS_SESSION_KEY];
        }

        public int CreateOrderForm(OrderForm form)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var forms = GetSessionForms(HttpContext.Current);
            int id = forms.Max(x => x.OrderFormId) + 1;
            form.OrderFormId = id;
            forms.Add(form);
            return id;
        }


        public void SaveOrderForm(OrderForm form)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var forms = GetSessionForms(HttpContext.Current);
            var oldForm = forms.FirstOrDefault(x => x.OrderFormId == form.OrderFormId);
            if(oldForm == null)
                throw new ArgumentException("OrderFormId");
            // старую удаляем, новую добавляем
            forms.Remove(oldForm);
            forms.Add(form);
        }

        public void DeleteOrderForm(int formId)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var forms = GetSessionForms(HttpContext.Current);
            var oldForm = forms.FirstOrDefault(x => x.OrderFormId == formId);
            if (oldForm == null)
                throw new ArgumentException("OrderFormId");
            forms.Remove(oldForm);
        }

        public OrderForm GetOrderForm(int formId)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var forms = GetSessionForms(HttpContext.Current);
            var form = forms.FirstOrDefault(x => x.OrderFormId == formId);
            return form;
        }

        public IList<OrderForm> GetOrderForms()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var forms = GetSessionForms(HttpContext.Current);
            return forms;
        }
    }
}
