using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoCMS.Base.Infrastructure
{
    public class MyDateTimeModelBinder : DefaultModelBinder
    {
        protected override object GetPropertyValue(ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            System.ComponentModel.PropertyDescriptor propertyDescriptor,
            IModelBinder propertyBinder)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime) || propertyDescriptor.PropertyType == typeof(DateTime?))
            {
                try
                {
                    var value = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
                    return DateTime.ParseExact(value.AttemptedValue, new string[] {"dd.MM.yyyy", "dd.MM.yyyy HH:mm"},
                        CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                catch
                {
                    if (propertyDescriptor.PropertyType == typeof (DateTime))
                    {
                        return default(DateTime);
                    }
                    else
                    {
                        return null;
                    }

                }
            }

            //let the default model binder do it's thing
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}
