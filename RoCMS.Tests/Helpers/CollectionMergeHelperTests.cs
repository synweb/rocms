using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RoCMS.Base.Helpers;
using RoCMS.Helpers;

namespace RoCMS.Tests.Helpers
{
    [TestFixture]
    public class CollectionMergeHelperTests
    {
        public class TestClass
        {
            public int Id { get; set; }
            public string Data { get; set; }
        }

        [Test]
        public void MergeTest()
        {
            var oldCollection = new List<TestClass>()
            {
                new TestClass() {Id = 1, Data = "first"},
                new TestClass() {Id = 2, Data = "second"},
                new TestClass() {Id = 3, Data = "third"},
                new TestClass() {Id = 10, Data = "tenth"},
                new TestClass() {Id = 100, Data = "hundredth"},
            };
            var newCollection = new List<TestClass>()
            {
                new TestClass() {Id = 1, Data = "firstNew"},
                new TestClass() {Id = 3, Data = "thirdNew"},
                new TestClass() {Id = 4, Data = "fourthNew"},
            };

            var resultCollection = oldCollection.ToList();

            bool Comparer(TestClass x, TestClass y)
            {
                return x.Id == y.Id;
            }

            int inserts = 0;
            int updates = 0;
            int deletes = 0;

            CollectionMergeHelper.MergeNewAndOld(
                newItems: newCollection, 
                existingItems: oldCollection, 
                comparer: Comparer,
                create: x =>
                {
                    resultCollection.Add(x);
                    inserts++;
                },
                update: (x) =>
                {
                    resultCollection.Single(y => Comparer(x, y)).Data = x.Data;
                    updates++;
                },
                delete: @class =>
                {
                    deletes++;
                    return resultCollection.Remove(@class);
                });

            Assert.AreEqual(resultCollection.Count, 3);
            Assert.AreEqual(resultCollection.Single(x => x.Id==1).Data, "firstNew");
            Assert.IsNull(resultCollection.SingleOrDefault(x => x.Id==2));
            Assert.AreEqual(resultCollection.Single(x => x.Id==3).Data, "thirdNew");
            Assert.AreEqual(resultCollection.Single(x => x.Id==4).Data, "fourthNew");
        }
    }
}
