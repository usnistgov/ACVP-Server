using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.cSHAKE
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
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldReturnSampleMonteCarloGeneratorIfRequested(bool isSample)
        {
            var testGroup = new TestGroup
            {
                Function = "cSHAKE",
                DigestSize = 128,
                TestType = "MCT",
                OutputLength = new MathDomain().AddSegment(new ValueDomainSegment(8))
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
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(
                new TestGroup
                {
                    Function = "cSHAKE",
                    DigestSize = 0,
                    TestType = "",
                    OutputLength = new MathDomain().AddSegment(new ValueDomainSegment(8))
                }
                );
            Assert.That(generator, Is.Not.Null);
        }

        private TestCaseGeneratorFactory GetSubject()
        {
            return new TestCaseGeneratorFactory(null, null);
        }
    }
}
