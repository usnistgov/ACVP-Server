using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        [TestCase("aFt", typeof(TestCaseGeneratorAft))]
        [TestCase("Mct", typeof(TestCaseGeneratorMct))]
        public void ShouldReturnProperSHA3Generator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = "SHA3",
                DigestSize = 224,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase("junk", typeof(TestCaseGeneratorNull))]
        [TestCase("aFt", typeof(TestCaseGeneratorAft))]
        [TestCase("Mct", typeof(TestCaseGeneratorShakeMct))]
        [TestCase("VOT", typeof(TestCaseGeneratorVot))]
        public void ShouldReturnProperSHAKEGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Function = "SHAKE",
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
        public async Task ShouldReturnSampleSHA3MonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "sha3",
                DigestSize = 224,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMct;
            Assume.That(typedGen != null);

            await typedGen.GenerateAsync(testGroup, isSample);

            Assert.AreEqual(isSample, typedGen.IsSample);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldReturnSampleSHAKEMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "shake",
                DigestSize = 128,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMct;
            Assume.That(typedGen != null);

            await typedGen.GenerateAsync(testGroup, isSample);

            Assert.AreEqual(isSample, typedGen.IsSample);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(
                new TestGroup
                {
                    Function = "SHA3",
                    DigestSize = 0,
                    TestType = ""}
                );
            Assert.IsNotNull(generator);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            return new TestCaseGeneratorFactory(null);
        }
    }
}
