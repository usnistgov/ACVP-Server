using System;
using NIST.CVP.Generation.KMAC.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseGeneratorFactory(null);
        }

        [Test]
        [TestCase("aFt", typeof(TestCaseGeneratorAft))]
        [TestCase("Mvt", typeof(TestCaseGeneratorMvt))]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup
            {
                TestType = testType
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
