using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB1.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CFB1
{
    [TestFixture, UnitTest]
    public class TestCaseTests
    {
        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("key")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.That(result, Is.True);

        }



        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.Key.ToHex(), Is.EqualTo("00AA"));
        }



        [Test]
        [TestCase("CipherText")]
        [TestCase("ciphertext")]
        [TestCase("ct")]
        [TestCase("CT")]
        public void ShouldSetCipherText(string name)
        {
            string text = "1";
            var expected = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(text));

            var subject = new TestCase();
            var result = subject.SetString(name, text);
            Assert.That(result, Is.True);
            Assert.That(subject.CipherText, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("PlainText")]
        [TestCase("PLAINtext")]
        [TestCase("pt")]
        [TestCase("PT")]
        public void ShouldSetPlainText(string name)
        {
            string text = "1";
            var expected = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed(text));

            var subject = new TestCase();
            var result = subject.SetString(name, text);
            Assert.That(result, Is.True);
            Assert.That(subject.PlainText, Is.EqualTo(expected));
        }
    }
}
