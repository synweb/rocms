using System;
using NUnit.Framework;
using RoCMS.Base.Exceptions;
using RoCMS.Base.Helpers;

namespace RoCMS.Tests.Helpers
{
    [TestFixture]
    public class FormattingHelperTests
    {
        [TestCase("+7903666-88-77", "79036668877", false)]
        [TestCase("-+7903666-88-77", "", true)]
        [TestCase("79036668877", "79036668877", false)]
        [TestCase("+79036668877", "79036668877", false)]
        [TestCase("+7 (903) 666-88-77", "79036668877", false)]
        [TestCase("(111)123-4567", "", true)]
        [TestCase("+38 (050) 123-45-67", "380501234567", true)]
        public void FormatPhoneTests(string @in, string @out, bool awaitException)
        {
            try
            {
                var res = FormattingHelper.FormatPhone(@in);
                Console.WriteLine(res);
                Assert.AreEqual(res, @out);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().Name);
                if (awaitException)
                {
                    Assert.That(e is PhoneValidationException);
                }
                else
                {
                    throw;
                }
            }
        }

        [TestCase("<a href=\"/\"></a>", "<a rel=\"nofollow\" href=\"/\"></a>", true)]
        [TestCase("<div></div>", "<div></div>", true)]
        [TestCase("<div><a href=\"/\"></a></div>", "<div><a rel=\"nofollow\" href=\"/\"></a></div>", true)]
        [TestCase("<a rel=\"nofollow\" href=\"/\"></a>", "<a rel=\"nofollow\" href=\"/\"></a>", true)]
        [TestCase("<a href=\"/\" rel=\"nofollow\"></a>", "<a href=\"/\" rel=\"nofollow\"></a>", true)]
        [TestCase("<a href=\"/\">ss</a><a href=\"/\" rel=\"nofollow\">ww</a>", "<a rel=\"nofollow\" href=\"/\">ss</a><a href=\"/\" rel=\"nofollow\">ww</a>", true)]
        public void AddRelNofollowToAnchorsTests(string @in, string @out, bool awaitException)
        {
            try
            {
                var res = FormattingHelper.AddRelNofollowToAnchors(@in);
                Console.WriteLine(res);
                Assert.AreEqual(res, @out);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().Name);
                if (awaitException)
                {
                    Assert.That(e is PhoneValidationException);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
