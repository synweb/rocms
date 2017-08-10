using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RoCMS.Tests.Data
{
    [TestFixture]
    public class EnumGatewayTests
    {
        private readonly IdStringEnumGateway _gateway = new IdStringEnumGateway();

        [Test]
        public void SelectTest()
        {
            var recs = _gateway.Select();

            Assert.That(recs.Count >= 2);
            Assert.That(recs.Any(x => x.Enum == DbEnum.First && x.Id == 1));
            Assert.That(recs.Any(x => x.Enum == DbEnum.Second && x.Id == 2));
        }

        [Test]
        public void InsertTest()
        {
            var rec = new IdStringEnum() {Enum = DbEnum.Third};
            int id = _gateway.Insert(rec);
            Assert.That(id >= 3);
        }


        [Test]
        public void SelectOneTest()
        {
            var rec = _gateway.SelectOne(1);

            Assert.That(rec.Id == 1 && rec.Enum == DbEnum.First);
        }



        [Test]
        public void SelectOneWithExecTest()
        {
            var rec = _gateway.SelectOneWithExec(1);

            Assert.That(rec.Id == 1 && rec.Enum == DbEnum.First);
        }

        [Test]
        public void SelectNothingTests()
        {
            var nullable = _gateway.SelectNothingAsNullableEnum();

            try
            {
                var @enum = _gateway.SelectNothingAsEnum();
            }
            catch (Exception e)
            {
                Assert.That(e is SqlNullValueException);
            }

            Assert.That(nullable == null);
        }
    }
}
