using System;
using NUnit.Framework;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Tests.Models
{
    [TestFixture]
    public class ThumbnailSizeTests
    {
        [Test]
        public void ParamConstructorTest()
        {
            var ts1 = new ThumbnailSize(200, ImageSide.Height);
            var ts2 = new ThumbnailSize(300, ImageSide.Width);

            Assert.AreEqual(200, ts1.Pixels);
            Assert.AreEqual(ImageSide.Height, ts1.Side);
            Assert.AreEqual(300, ts2.Pixels);
            Assert.AreEqual(ImageSide.Width, ts2.Side);
        }

        [Test]
        [TestCase("300w", 300, ImageSide.Width)]
        [TestCase("400h", 400, ImageSide.Height)]
        public void StringConstructorTest(string input, int pixels, ImageSide side)
        {
            var ts = new ThumbnailSize(input);

            Assert.AreEqual(pixels, ts.Pixels);
            Assert.AreEqual(side, ts.Side);
        }

        [Test]
        [TestCase("400hd", typeof(FormatException))]
        [TestCase("", typeof(FormatException))]
        [TestCase(null, typeof(ArgumentNullException))]
        public void StringConstructorFailingTest(string input, Type exceptionType)
        {
            Assert.That(() => new ThumbnailSize(input), Throws.InstanceOf(exceptionType));
        }

        [Test]
        [TestCase(700, ImageSide.Width, "700w")]
        [TestCase(850, ImageSide.Width, "850w")]
        [TestCase(44, ImageSide.Height, "44h")]
        public void SizeIdCreationTest(int pix, ImageSide side, string id)
        {
            var ts = new ThumbnailSize(pix, side);
            Assert.AreEqual(id, ts.SizeString);
        }
    }
}
