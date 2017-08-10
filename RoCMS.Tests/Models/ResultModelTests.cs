using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RoCMS.Base.Models;

namespace RoCMS.Tests.Models
{
    [TestFixture]
    public class ResultModelTests
    {
        [Test]
        public void ExceptionConstructorTest()
        {
            var e1 = new NotSupportedException();
            var e2 = new DataMisalignedException("qwe");

            var rm1 = new ResultModel(e1);
            var rm2 = new ResultModel(e2);

            Assert.That(!rm1.Succeed);
            Assert.That(rm1.ErrorType == "NotSupported");
            Assert.That(!rm2.Succeed);
            Assert.That(rm2.ErrorType == "DataMisaligned");
        }
    }
}
