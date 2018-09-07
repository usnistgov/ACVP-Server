using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGenerator))]
        [TestCase("Encrypt", typeof(TestCaseGenerator))]
        [TestCase("Encrypt", typeof(TestCaseGenerator))]
        [TestCase("ENcrypt", typeof(TestCaseGenerator))]
        [TestCase("Decrypt", typeof(TestCaseGenerator))]
        [TestCase("decrypt", typeof(TestCaseGenerator))]
        [TestCase("decrypt", typeof(TestCaseGenerator))]
        [TestCase("decrypt", typeof(TestCaseGenerator))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = string.Empty,
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
