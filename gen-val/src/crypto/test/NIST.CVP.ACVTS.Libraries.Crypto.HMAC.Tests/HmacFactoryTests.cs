using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.HMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class HmacFactoryTests
    {
        private HmacFactory _subject;

        [SetUp]
        public void Setup()
        {
            // Can't mock this up easily, needs properties from an actual HashFunction
            _subject = new HmacFactory(new NativeShaFactory());
        }

        [Test]
        public void ShouldReturnHmacInstance()
        {
            var result = _subject.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));

            Assert.IsInstanceOf(typeof(NativeHmac), result);
        }
    }
}
