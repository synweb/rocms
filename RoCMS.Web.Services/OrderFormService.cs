using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class OrderFormService: IOrderFormService
    {
        OrderFormFieldGateway _orderFormFieldGateway = new OrderFormFieldGateway();
        OrderFormGateway _orderFormGateway = new OrderFormGateway();

        public int CreateOrderForm(OrderForm form)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var dataForm = Mapper.Map<Data.Models.OrderForm>(form);
                int id = _orderFormGateway.Insert(dataForm);
                var fields = Mapper.Map<ICollection<Data.Models.OrderFormField>>(form.Fields);
                foreach (var field in fields)
                {
                    field.OrderFormId = id;
                    _orderFormFieldGateway.Insert(field);
                }

                ts.Complete();

                return id;
            }
        }

        public void SaveOrderForm(OrderForm form)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var dataForm = Mapper.Map<Data.Models.OrderForm>(form);
                _orderFormGateway.Update(dataForm);

                var fields = Mapper.Map<ICollection<Data.Models.OrderFormField>>(form.Fields);
                var oldFields = _orderFormFieldGateway.SelectByForm(form.OrderFormId);

                foreach (var orderFormField in oldFields.Where(x => fields.All(y => x.OrderFormFieldId != y.OrderFormFieldId))) //удаление
                {
                    _orderFormFieldGateway.Delete(orderFormField.OrderFormFieldId);
                }

                foreach (var orderFormField in fields.Where(x => oldFields.All(y => x.OrderFormFieldId != y.OrderFormFieldId))) //добавление
                {
                    orderFormField.OrderFormId = form.OrderFormId;
                    _orderFormFieldGateway.Insert(orderFormField);
                }

                foreach (var orderFormField in fields.Where(x => oldFields.Any(y => x.OrderFormFieldId == y.OrderFormFieldId))) //обновление
                {
                    _orderFormFieldGateway.Update(orderFormField);
                }

                ts.Complete();

            }
        }

        public void DeleteOrderForm(int formId)
        {
            _orderFormGateway.Delete(formId);
        }

        public OrderForm GetOrderForm(int formId)
        {
            var dataForm = _orderFormGateway.SelectOne(formId);
            var result = Mapper.Map<OrderForm>(dataForm);
            var dataFields = _orderFormFieldGateway.SelectByForm(formId);

            result.Fields = Mapper.Map<List<OrderFormField>>(dataFields.OrderBy(x => x.SortOrder));

            return result;
        }

        public IList<OrderForm> GetOrderForms()
        {
            var dataForms = _orderFormGateway.Select();
            var forms = Mapper.Map<IList<OrderForm>>(dataForms);
            return forms;
        }
    }
}
