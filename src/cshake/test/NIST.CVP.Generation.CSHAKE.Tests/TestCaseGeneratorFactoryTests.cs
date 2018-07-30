using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CSHAKE.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        [TestCase("aFt", typeof(TestCaseGeneratorAft))]
        [TestCase("Mct", typeof(TestCaseGeneratorMct))]
        public void ShouldReturnProperGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = "cSHAKE",
                DigestSize = 128,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnSampleMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "cSHAKE",
                DigestSize = 128,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMct;
            Assume.That(typedGen != null);

            var result = typedGen.Generate(testGroup, isSample);

            Assert.AreEqual(isSample, typedGen.IsSample);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(
                new TestGroup
                {
                    Function = "cSHAKE",
                    DigestSize = 0,
                    TestType = ""
                }
                );
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            return new TestCaseGeneratorFactory(null);
        }
    }
}
