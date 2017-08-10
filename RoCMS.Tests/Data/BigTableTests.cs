using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RoCMS.Tests.Data
{
    [TestFixture]
    public class BigTableTests
    {
        private readonly RandomIntsGateway _gateway = new RandomIntsGateway();

        [Test]
        public void BigSelectTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var recs = _gateway.Select();
            sw.Stop();
            Assert.That(recs.Count >= 20000);
            Console.WriteLine($"{sw.Elapsed.TotalMilliseconds} ms passed");
        }


        [Test]
        public void ManySelectOnesTest()
        {
            const int COUNT = 500;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var recs = _gateway.SelectManyBySelectOne(COUNT);
            sw.Stop();
            Assert.That(recs.Count == COUNT);
            Console.WriteLine($"{sw.Elapsed.TotalMilliseconds} ms passed");
        }
    }
}
