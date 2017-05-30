using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("Reseed PredResist", true, true, typeof(TestCaseGeneratorReseedPredResist))]
        [TestCase("Reseed No PredResist", true, false, typeof(TestCaseGeneratorReseedNoPredResist))]
        [TestCase("No Reseed PredResist", false, true, typeof(TestCaseGeneratorNoReseed))]
        [TestCase("No Reseed No PredResist", false, false, typeof(TestCaseGeneratorNoReseed))]
        public void ShouldReturnProperGenerator(string label, bool reseed, bool predResist, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                ReSeed = reseed,
                PredResistance = predResist
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
