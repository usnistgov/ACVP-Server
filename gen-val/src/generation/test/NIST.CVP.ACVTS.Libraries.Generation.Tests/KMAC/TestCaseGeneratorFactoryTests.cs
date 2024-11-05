using System;
using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KMAC
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseGeneratorFactory(null, null);
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
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }
    }
}
