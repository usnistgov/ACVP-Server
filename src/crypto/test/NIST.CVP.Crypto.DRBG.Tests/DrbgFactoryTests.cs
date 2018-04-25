
using System;
using Moq;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture,  FastCryptoTest]
    public class DrbgFactoryTests
    {
        private Mock<IEntropyProvider> _mockEntropy;
        private DrbgFactory _subject;
        
        [SetUp]
        public void Setup()
        {
            _mockEntropy = new Mock<IEntropyProvider>();
            _subject = new DrbgFactory();
        }

        [Test]
        [TestCase(DrbgMechanism.Counter, DrbgMode.AES128, typeof(DrbgCounterAes))]
        [TestCase(DrbgMechanism.Counter, DrbgMode.AES192, typeof(DrbgCounterAes))]
        [TestCase(DrbgMechanism.Counter, DrbgMode.AES256, typeof(DrbgCounterAes))]
        [TestCase(DrbgMechanism.Counter, DrbgMode.TDES, typeof(DrbgCounterTdes))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA1, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA224, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA256, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA384, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA512, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA512t224, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.Hash, DrbgMode.SHA512t256, typeof(DrbgHash))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA1, typeof(DrbgHmac))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA224, typeof(DrbgHmac))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA256, typeof(DrbgHmac))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA384, typeof(DrbgHmac))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA512, typeof(DrbgHmac))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA512t224, typeof(DrbgHmac))]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.SHA512t256, typeof(DrbgHmac))]
        public void ShouldReturnCorrectType(DrbgMechanism drbgMechanism, DrbgMode drbgMode, Type expectedType)
        {
            DrbgParameters p = new DrbgParameters();
            p.Mechanism = drbgMechanism;
            p.Mode = drbgMode;

            var result = _subject.GetDrbgInstance(p, _mockEntropy.Object);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidMechanismEnum()
        {
            DrbgParameters p = new DrbgParameters();
            int i = -1;
            var badMechanism = (DrbgMechanism) i;
            p.Mechanism = badMechanism;
            p.Mode = DrbgMode.AES128;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetDrbgInstance(p, _mockEntropy.Object));
        }
        
        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidModeEnum()
        {
            DrbgParameters p = new DrbgParameters();
            int i = -1;
            var badMode = (DrbgMode)i;
            p.Mechanism = DrbgMechanism.Counter;
            p.Mode = badMode;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetDrbgInstance(p, _mockEntropy.Object));
        }

        [Test]
        [TestCase(DrbgMechanism.Counter, DrbgMode.SHA1)]
        [TestCase(DrbgMechanism.Counter, DrbgMode.SHA512t224)]
        [TestCase(DrbgMechanism.Hash, DrbgMode.TDES)]
        [TestCase(DrbgMechanism.Hash, DrbgMode.AES128)]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.AES192)]
        [TestCase(DrbgMechanism.HMAC, DrbgMode.AES256)]
        public void ShouldReturnArgumentExceptionWithNonMatchingEnums(DrbgMechanism mechanism, DrbgMode mode)
        {
            var p = new DrbgParameters
            {
                Mechanism = mechanism,
                Mode = mode
            };

            Assert.Throws(typeof(ArgumentException), () => _subject.GetDrbgInstance(p, _mockEntropy.Object));
        }
    }
}

