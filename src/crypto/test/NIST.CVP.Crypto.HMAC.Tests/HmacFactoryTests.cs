using Moq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.HMAC.Tests
{
    [TestFixture,  FastCryptoTest]
    public class HmacFactoryTests
    {
        private HmacFactory _subject;
        private Mock<IShaFactory> _shaFactory;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new Mock<IShaFactory>();
            _subject = new HmacFactory(_shaFactory.Object);
        }

        public void ShouldReturnHmacInstance()
        {
            var result = _subject.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));

            Assert.IsInstanceOf(typeof(Hmac), result);
        }
    }
}