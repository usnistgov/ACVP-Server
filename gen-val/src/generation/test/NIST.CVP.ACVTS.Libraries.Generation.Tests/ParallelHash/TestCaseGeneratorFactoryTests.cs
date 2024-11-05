using System;
using NIST.CVP.ACVTS.Libraries.Generation.ParallelHash.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ParallelHash
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
                Function = "ParallelHash",
                DigestSize = 128,
                TestType = testType
            };

            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var subject = GetSubject();
            var generator = subject.GetCaseGenerator(
                new TestGroup
                {
                    Function = "ParallelHash",
                    DigestSize = 0,
                    TestType = ""
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
