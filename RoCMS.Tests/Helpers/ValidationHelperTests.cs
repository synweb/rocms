using NUnit.Framework;
using RoCMS.Base.Helpers;

namespace RoCMS.Tests.Helpers
{
    [TestFixture]
    public class ValidationHelperTests
    {
        [TestCase("qwe@qwe.ru", true)]
        [TestCase("order@synweb.ru", true)]
        [TestCase("order@synweb", false)]
        [TestCase("order", false)]
        [TestCase("@synweb.ru", false)]
        [TestCase("@synweb", false)]
        [TestCase("aa@ru", false)]
        public void ValidateEmailTests(string email, bool isValid)
        {
            Assert.That(ValidationHelper.ValidateEmail(email) == isValid);
        }

        [TestCase("", false)]
        [TestCase("+7 495 123-45-67", true)]
        [TestCase("+7 495 1234567", true)]
        [TestCase("+7 (495) 123-45-67", true)]
        [TestCase("+74951234567", true)]
        [TestCase("+380671234567", true)]
        [TestCase("+38 456 123123123123123123", false)]
        [TestCase("7 495 123-45-67", true)]
        [TestCase("7 495 1234567", true)]
        [TestCase("7 (495) 123-45-67", true)]
        [TestCase("74951234567", true)]
        [TestCase("380671234567", true)]
        [TestCase("+38 (050) 123-45-67", true)]
        [TestCase("38 456 123123123123123123", false)]
        [TestCase("-25423423434", false)]
        [TestCase("ksdlksfjdslkfjdl", false)]
        [TestCase("+12345", false)]
        [TestCase("+1234567890", true)]
        public void ValidatePhoneTests(string phone, bool isValid)
        {
            Assert.That(ValidationHelper.ValidatePhone(phone) == isValid);
        }

        [TestCase("+12345,67890", false)]
        [TestCase("890", true)]
        [TestCase("890,890", false)]
        [TestCase("890,891", true)]
        [TestCase(" 890,891", false)]
        [TestCase("ы", false)]
        [TestCase(" ы", false)]
        [TestCase("ы ", false)]
        [TestCase("ыы", true)]
        [TestCase("ыы,яя,33вв", true)]
        [TestCase("первый,второй,", false)]
        [TestCase("ыы,яя ,33вв", false)]
        [TestCase("app store,apple store,модерация,премодерация", true)]
        [TestCase("уран-тогоо,вики", true)]
        public void ValidateTagsTests(string tags, bool isValid)
        {
            Assert.That(ValidationHelper.ValidateTags(tags) == isValid);
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("    ", false)]
        [TestCase("200w", true)]
        [TestCase("300h", true)]
        [TestCase("400w,500h", true)]
        [TestCase("600h, 700h", false)]
        [TestCase("300w ,400w,   500h,20h", false)]
        [TestCase("300t", false)]
        [TestCase("300td", false)]
        [TestCase("300td,", false)]
        [TestCase("300w,400w,500h,20h,", false)]
        [TestCase("w800", false)]
        [TestCase("w800,400h", false)]
        [TestCase("30h,80w,30h", false)]
        [TestCase("30h,30w", true)]
        public void ValidateThumbnailSizesTests(string sizes, bool isValid)
        {
            Assert.AreEqual(isValid, ValidationHelper.ValidateThumbnailSizes(sizes));
        }
    }
}
