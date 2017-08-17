using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RoCMS.Base.Exceptions;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Data.Mapping
{
    public class DataMapperService: IMapperService
    {
        public T Map<T>(object source)
        {
            return (T)Mapper.Map(source, source.GetType(), typeof(T));
        }

        public void CreateTwoWayMap<T1, T2>()
        {
            Mapper.CreateMap<T1, T2>();
            Mapper.CreateMap<T2, T1>();
            Mapper.AssertConfigurationIsValid();
        }

        public void CreateMap<T1, T2>()
        {
            Mapper.CreateMap<T1, T2>();
            Mapper.AssertConfigurationIsValid();
        }

        public void CreateMap<T1, T2>(Func<T1,T2> converter)
        {
            Mapper.CreateMap<T1, T2>().ConvertUsing(converter);
            Mapper.AssertConfigurationIsValid();
        }

        public void CreateMap<T1, T2>(IList<string> ignore)
        {
            if (ignore.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException("Название связываемого свойства должно быть указано");
            }
            var expr = Mapper.CreateMap<T1, T2>();
                foreach (string item in ignore)
                {
                    try
                    {
                        expr.ForMember(item, x => x.Ignore());
                    }
                    catch (Exception)
                    {
                        throw new MappingException(typeof (T1), typeof (T2),
                                                   String.Format("Ошибка при игнорировании свойства {0} в классе {1}",
                                                                 item, typeof (T1).Name));
                    }
                }
            Mapper.AssertConfigurationIsValid();
        }
    }
}
