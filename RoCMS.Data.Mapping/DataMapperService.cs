using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        //не получилось :(
        //public void CreateMap<T1, T2>(Dictionary<string, string> mapFromTo, IList<string> ignore)
        //{
        //    if (mapFromTo.Keys.Any(string.IsNullOrEmpty) || mapFromTo.Values.Any(string.IsNullOrEmpty) || ignore.Any(string.IsNullOrEmpty))
        //    {
        //        throw new ArgumentException("Название связываемого свойства должно быть указано");
        //    }
        //    var expr = Mapper.CreateMap<T1, T2>();

        //    foreach (var item in mapFromTo)
        //    {
        //        string from = item.Key;
        //        string to = item.Value;
        //        //вот тут проблема... преобразование к object
        //        expr.ForMember(from, x => x.MapFrom(opt => opt.GetType().GetProperty(to).GetValue(opt)));
        //    }

        //    foreach(string item in ignore)
        //    {
        //        expr.ForMember(item, x => x.Ignore());
        //    }

        //    Mapper.AssertConfigurationIsValid();
        //}

        //не получилось :(
        //public void CreateTwoWayMap<T1, T2>(Dictionary<string, string> mapFromTo)
        //{
        //    var fromTo = Mapper.CreateMap<T1, T2>();
        //    var toFrom = Mapper.CreateMap<T2, T1>();

        //    foreach (var item in mapFromTo)
        //    {
        //        string from = item.Key;
        //        string to = item.Value;
        //        fromTo.ForMember(to, x => x.MapFrom(opt => opt.GetType().GetProperty(from).GetValue(opt)));
        //        toFrom.ForMember(from, x => x.MapFrom(opt =>
        //                                              {
        //                                                  var type = opt.GetType().GetProperty(to).PropertyType;
        //                                                  var value = opt.GetType().GetProperty(to).GetValue(opt);
                                                          
        //                                              }));
        //    }

        //    Mapper.AssertConfigurationIsValid();
        //}
    }
}
