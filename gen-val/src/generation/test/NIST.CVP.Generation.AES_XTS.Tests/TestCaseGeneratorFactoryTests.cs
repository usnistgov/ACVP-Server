using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using NIST.CVP.Generation.AES_XTS.v1_0;

namespace NIST.CVP.Generation.AES_XTS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGenerator))]
        [TestCase("Encrypt", typeof(TestCaseGenerator))]
        [TestCase("ENcrypt", typeof(TestCaseGenerator))]
        [TestCase("Decrypt", typeof(TestCaseGenerator))]
        [TestCase("decrypt", typeof(TestCaseGenerator))]
        [TestCase("dECrypt", typeof(TestCaseGenerator))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup
            {
                Direction = direction,
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup
            {
                Direction = string.Empty,
                TestType = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
