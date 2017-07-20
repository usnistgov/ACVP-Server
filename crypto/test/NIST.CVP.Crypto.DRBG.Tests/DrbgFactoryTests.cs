
using System;
using Moq;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture, UnitTest]
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
    }
}
