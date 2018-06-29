using System;
using Moq;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.KMAC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private Mock<IRandom800_90> _random;
        private Mock<IKmac> _algo;
        private Mock<IKmacFactory> _algoFactory;
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _algo = new Mock<IKmac>();
            _algoFactory = new Mock<IKmacFactory>();
            _subject = new TestCaseGeneratorFactory(_random.Object, _algoFactory.Object);

            _algoFactory
                .Setup(s => s.GetKmacInstance(256, false))
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
