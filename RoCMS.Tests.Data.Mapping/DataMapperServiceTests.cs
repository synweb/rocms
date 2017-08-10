
using System;
using System.Collections.Generic;
using NUnit.Framework;
using RoCMS.Data.Mapping;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Tests.Data.Mapping
{
    [TestFixture]
    public class DataMapperServiceTests
    {
        private IMapperService _mapper = new DataMapperService();

        private class SimpleClass1
        {
            public int Int { get; set; }
            public string Str { get; set; }
        }

        private class SimpleClass2
        {
            public int Int { get; set; }
            public string Str { get; set; }
        }

        private class SimpleClass3
        {
            public int Int2 { get; set; }
            public string Str2 { get; set; }
        }

        private class SimpleClass4
        {
            public int Int { get; set; }
            public string Str { get; set; }
            public bool Bool { get; set; }
        }

        private class SimpleClass5
        {
            public int Int { get; set; }
            public string Str { get; set; }
            public bool Bool { get; set; }
        }

        [Test]
        public void CreateSimpleMapTest()
        {
            SimpleClass1 c = new SimpleClass1() {Int = 8, Str = "123"};
            SimpleClass4 c4 = new SimpleClass4() { Int = 8, Str = "123", Bool=true };

            _mapper.CreateMap<SimpleClass1, SimpleClass2>();
            _mapper.CreateMap<SimpleClass4, SimpleClass1>();
            SimpleClass2 c2 = _mapper.Map<SimpleClass2>(c);
            SimpleClass1 c1 = _mapper.Map<SimpleClass1>(c4);

            Assert.AreEqual(c.Int, c2.Int);
            Assert.AreEqual(c.Str, c2.Str);
            Assert.AreEqual(c.Int, c4.Int);
            Assert.AreEqual(c.Str, c4.Str);
            Assert.Catch<Exception>(() =>
            {
                
                SimpleClass4 c41 = _mapper.Map<SimpleClass4>(c);
            });
        }

        [Test]
        public void CreateMapWithConverterTest()
        {
            SimpleClass1 c = new SimpleClass1() { Int = 8, Str = "123" };

            _mapper.CreateMap<SimpleClass1, SimpleClass3>((from) =>
            {
                SimpleClass3 res = new SimpleClass3()
                {
                    Int2 = from.Int,
                    Str2 = from.Str
                };
                return res;
            });
            SimpleClass3 c2 = _mapper.Map<SimpleClass3>(c);

            Assert.AreEqual(c.Int, c2.Int2);
            Assert.AreEqual(c.Str, c2.Str2);
        }

        [Test]
        public void CreateTwoWayMapTest()
        {
            SimpleClass1 c = new SimpleClass1() { Int = 8, Str = "123" };

            _mapper.CreateTwoWayMap<SimpleClass1, SimpleClass2>();
            SimpleClass2 c2 = _mapper.Map<SimpleClass2>(c);
            SimpleClass1 c3 = _mapper.Map<SimpleClass1>(c2);

            Assert.AreEqual(c.Int, c2.Int);
            Assert.AreEqual(c.Str, c2.Str);
            Assert.AreEqual(c.Int, c3.Int);
            Assert.AreEqual(c.Str, c3.Str);
        }

        [Test]
        public void CreateMapWithIgnoreTest()
        {
            SimpleClass4 c4 = new SimpleClass4() { Int = 8, Str = "123", Bool = true };

            _mapper.CreateMap<SimpleClass4, SimpleClass5>(new List<string>{"Str"});
            SimpleClass5 c5 = _mapper.Map<SimpleClass5>(c4);

            Assert.AreEqual(c4.Int, c5.Int);
            Assert.AreEqual(c4.Bool, c5.Bool);
            Assert.True(string.IsNullOrEmpty(c5.Str));
            Assert.Catch<Exception>(() =>
            {
                _mapper.CreateMap<SimpleClass4, SimpleClass5>(new List<string> {""});
            });
            Assert.Catch<Exception>(() =>
            {
                _mapper.CreateMap<SimpleClass4, SimpleClass5>(new List<string> { "Double" });
            });
        }
    }
}
