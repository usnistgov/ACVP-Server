using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _subject = new TestCaseGeneratorFactory(_oracle.Object);
        }

        [Test]
        [TestCase(typeof(TestCaseGenerator))]
        public void ShouldReturnProperGenerator(Type expectedType)
        {
            TestGroup testGroup = new TestGroup();

            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
