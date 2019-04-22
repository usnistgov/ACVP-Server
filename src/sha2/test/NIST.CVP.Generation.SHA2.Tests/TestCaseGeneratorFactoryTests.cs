using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.SHA2.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, "aFt", typeof(TestCaseGeneratorAFTHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "AFT", typeof(TestCaseGeneratorAFTHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "MCT", typeof(TestCaseGeneratorMCTHash))]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "mCt", typeof(TestCaseGeneratorMCTHash))]
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
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldReturnSampleMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup()
            {
                Function = ModeValues.SHA1,
                DigestSize = DigestSizes.d160,
                TestType = "mct"
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);

            var typedGen = generator as TestCaseGeneratorMCTHash;
            Assume.That(generator != null);

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
                    Function = 0,
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
