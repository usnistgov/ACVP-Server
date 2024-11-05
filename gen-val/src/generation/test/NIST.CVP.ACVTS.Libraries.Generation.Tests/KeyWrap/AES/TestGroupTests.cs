using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.AES
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
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("KeyLen")]
        [TestCase("KEYLEN")]
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.That(result, Is.True);
            Assert.That(subject.KeyLength, Is.EqualTo(13));
        }

        [Test]
        [TestCase("ptLen")]
        [TestCase("PTLEN")]
        public void ShouldSetPtLenth(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.That(result, Is.True);
            Assert.That(subject.PayloadLen, Is.EqualTo(13));
        }

        [Test]
        [TestCase("cipher", false)]
        [TestCase("inverse", true)]
        public void ShouldGetUseInverseCipherProperly(string kwCipher, bool expectationUseInverseCipher)
        {
            TestGroup tg = new TestGroup();
            tg.KwCipher = kwCipher;

            var result = tg.UseInverseCipher;

            Assert.That(result, Is.EqualTo(expectationUseInverseCipher));
        }
    }
}
