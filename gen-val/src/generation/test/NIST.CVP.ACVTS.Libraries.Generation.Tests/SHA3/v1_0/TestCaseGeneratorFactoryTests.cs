using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v1_0
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
                Function = ModeValues.SHA3,
                DigestSize = DigestSizes.d224,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
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
                Function = ModeValues.SHAKE,
                DigestSize = DigestSizes.d128,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldReturnSampleSHA3MonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = ModeValues.SHA3,
                DigestSize = DigestSizes.d224,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMct;
            Assert.That(typedGen != null);

            await typedGen.GenerateAsync(testGroup, isSample);

            Assert.That(typedGen.IsSample, Is.EqualTo(isSample));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldReturnSampleSHAKEMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = ModeValues.SHAKE,
                DigestSize = DigestSizes.d128,
                TestType = "MCT"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);

            var typedGen = generator as TestCaseGeneratorShakeMct;
            Assert.That(typedGen != null);

            await typedGen.GenerateAsync(testGroup, isSample);

            Assert.That(typedGen.IsSample, Is.EqualTo(isSample));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(
                new TestGroup
                {
                    Function = ModeValues.SHA1,
                    DigestSize = 0,
                    TestType = ""
                });
            Assert.That(generator, Is.Not.Null);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            return new TestCaseGeneratorFactory(null);
        }
    }
}
