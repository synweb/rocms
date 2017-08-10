using System;
using System.Collections.Generic;

namespace RoCMS.Web.Contract.Services
{
    /// <summary>
    /// Сервис конвертирования объектов из одного типа в другой
    /// </summary>
    public interface IMapperService
    {
        T Map<T>(object source);
        void CreateTwoWayMap<T1, T2>();
        void CreateMap<T1, T2>();
        void CreateMap<T1, T2>(Func<T1, T2> converter);
        void CreateMap<T1, T2>(IList<string> ignore);
    }
}
