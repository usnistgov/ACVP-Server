using System;
using Moq;
using NIST.CVP.Common.Oracle;
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
            Mock<IOracle> oracle = new Mock<IOracle>();

            _subject = new TestCaseGeneratorFactory(oracle.Object);
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