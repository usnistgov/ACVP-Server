using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests.AES
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "1");
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("KeyLen")]
        [TestCase("KEYLEN")]
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.KeyLength);
        }

        [Test]
        [TestCase("ptLen")]
        [TestCase("PTLEN")]
        public void ShouldSetPtLenth(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.PayloadLen);
        }

        [Test]
        [TestCase("cipher", false)]
        [TestCase("inverse", true)]
        public void ShouldGetUseInverseCipherProperly(string kwCipher, bool expectationUseInverseCipher)
        {
            TestGroup tg = new TestGroup();
            tg.KwCipher = kwCipher;

            var result = tg.UseInverseCipher;

            Assert.AreEqual(expectationUseInverseCipher, result);
        }
    }
}
