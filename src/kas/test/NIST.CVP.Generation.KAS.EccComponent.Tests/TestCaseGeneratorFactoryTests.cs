using System;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.EccComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            Mock<IEccCurveFactory> curveFactory = new Mock<IEccCurveFactory>();
            curveFactory
                .Setup(s => s.GetCurve(It.IsAny<Curve>()))
                .Returns(It.IsAny<IEccCurve>());
            Mock<IDsaEccFactory> dsaFactory = new Mock<IDsaEccFactory>();
            Mock<IEccDhComponent> dh = new Mock<IEccDhComponent>();

            _subject = new TestCaseGeneratorFactory(curveFactory.Object, dsaFactory.Object, dh.Object);
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