using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XECDH.RFC7748.SSC
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
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }
    }
}
