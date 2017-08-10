using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.FAQ.Contract.Models;

namespace RoCMS.FAQ.Services
{
    public abstract class BaseFAQSerivce
    {
        static BaseFAQSerivce()
        {
            Mapper.CreateMap<Question, Data.Models.Question>();
            Mapper.CreateMap<Data.Models.Question, Question>();
        }
    }
}
