using System;
using Moq;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private Mock<IRandom800_90> _random;
        private Mock<IHmac> _algo;
        private Mock<IHmacFactory> _algoFactory;
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _algo = new Mock<IHmac>();
            _algoFactory = new Mock<IHmacFactory>();
            _subject = new TestCaseGeneratorFactory(_random.Object, _algoFactory.Object);

            _algoFactory
                .Setup(s => s.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160)))
                .Returns(_algo.Object);
        }

        [Test]
        [TestCase(typeof(TestCaseGenerator))]
        public void ShouldReturnProperGenerator(Type expectedType)
        {
            TestGroup testGroup = new TestGroup();

            var generator = _subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
