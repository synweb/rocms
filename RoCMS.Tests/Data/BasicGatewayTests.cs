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
    public class BasicGatewayTests
    {
        private readonly IdValueGateway _gateway = new IdValueGateway();

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void SelectTest()
        {
            var recs = _gateway.Select();

            Assert.That(recs.Count >= 2);
            Assert.That(recs.Any(x => x.Value == "First" && x.Id == 1));
            Assert.That(recs.Any(x => x.Value == "Second" && x.Id == 2));
        }

        [Test]
        public void SelectWithOnlyParamTest()
        {
            var recs = _gateway.SelectWithIdLessThan(3);

            Assert.That(recs.Count == 2);
            Assert.That(recs.Any(x => x.Value == "First" && x.Id == 1));
            Assert.That(recs.Any(x => x.Value == "Second" && x.Id == 2));
        }


        [Test]
        public void SelectWithAnonymousParam()
        {
            var recs = _gateway.SelectByIdAndValue(1, "First");

            Assert.That(recs.Count == 1);
            var rec = recs.Single();
            Assert.That(rec.Value == "First" && rec.Id == 1);
        }

        [Test]
        public void InsertTest()
        {
            var rec = new IdValue() {Value = "The new one"};

            int id = _gateway.Insert(rec);

            Assert.That(id >= 3);
        }

        [Test]
        public void DeleteTest()
        {
            var recs = _gateway.Select().Where(x => x.Value.Equals("The new one"));
            if (!recs.Any())
            {
                var rec = new IdValue() { Value = "The new one" };
                _gateway.Insert(rec);
            }

            recs = _gateway.Select().Where(x => x.Value.Equals("The new one")).ToList();
            foreach (var idValue in recs)
            {
                _gateway.Delete(idValue.Id);
            }

            var afterDelete = _gateway.Select().Where(x => x.Value.Equals("The new one")).ToList();

            Assert.That(recs.Any());
            Assert.That(!afterDelete.Any());
        }

        [Test]
        public void ExecWithoutParamsTest()
        {
            int res = _gateway.SelectNine();

            Assert.That(res == 9);
        }
        
        [Test]
        public void ExecWithOneParamTest()
        {
            int? @null = _gateway.SelectNullOrFive(true);
            int? five = _gateway.SelectNullOrFive(false);

            Assert.That(@null == null);
            Assert.That(five == 5);
        }

        [Test]
        public void SelectNullableIntParamTest()
        {
            int? @null = _gateway.SelectNullableIntParam(null);
            int? seven = _gateway.SelectNullableIntParam(7);

            Assert.That(@null == null);
            Assert.That(seven == 7);
        }

        [Test]
        public void SelectManyNullablesTest()
        {
            ICollection<int?> res = _gateway.SelectIdsWithNull();

            Assert.That(res.Contains(null));
            Assert.That(res.Contains(1));
            Assert.That(res.Contains(2));
        }

        [Test]
        public void SelectOneNullableTest()
        {
            int? @null = _gateway.SelectNullOrFiveWithSelectOne(true);
            int? five = _gateway.SelectNullOrFiveWithSelectOne(false);

            Assert.That(@null == null);
            Assert.That(five == 5);
        }

        [Test]
        public void SelectNothingTest()
        {
            var nullable = _gateway.SelectNothingAsNullbale();

            try
            {
                var @int = _gateway.SelectNothingAsInt();
            }
            catch (Exception e)
            {
                Assert.That(e is SqlNullValueException);
            }

            var obj = _gateway.SelectNothingAsObject();

            Assert.That(nullable == null);
            Assert.That(obj == null);
        }
    }
}
