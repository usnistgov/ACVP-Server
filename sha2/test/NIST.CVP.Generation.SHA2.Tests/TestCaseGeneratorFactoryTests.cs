using System;
using Moq;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "longmessage", typeof(TestCaseGeneratorLongHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "LongMessage", typeof(TestCaseGeneratorLongHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "MONTECARLO", typeof(TestCaseGeneratorMonteCarloHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "MonteCarlo", typeof(TestCaseGeneratorMonteCarloHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, "ShOrTmEsSaGe", typeof(TestCaseGeneratorShortHash))]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "junk", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(ModeValues mode, DigestSizes size, string testType, Type expectedType)
        {
            var testGroup = new TestGroup()
            {
                Function = mode,
                DigestSize = size,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup, false);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnSampleMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup()
            {
                Function = ModeValues.SHA1,
                DigestSize = DigestSizes.d160,
                TestType = "MonteCarlo"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup, isSample);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMonteCarloHash;
            Assume.That(generator != null);
            Assert.AreEqual(isSample, typedGen.IsSample);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(new TestGroup { Function = 0, DigestSize = 0, TestType = "" }, false);
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            var random = new Mock<IRandom800_90>().Object;
            var algo = new Mock<ISHA>().Object;

            return new TestCaseGeneratorFactory(random, algo);
        }
    }
}
