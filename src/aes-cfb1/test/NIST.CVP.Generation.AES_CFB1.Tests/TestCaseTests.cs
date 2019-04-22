using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.AES_CFB1.v1_0;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
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
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("key")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.IsTrue(result);
            
        }

    

        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Key.ToHex());
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
            Assert.IsTrue(result);
            Assert.AreEqual(expected, subject.CipherText);
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
            Assert.IsTrue(result);
            Assert.AreEqual(expected, subject.PlainText);
        }
    }
}
