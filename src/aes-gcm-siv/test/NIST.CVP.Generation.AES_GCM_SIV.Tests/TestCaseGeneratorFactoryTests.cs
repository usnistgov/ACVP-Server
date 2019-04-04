using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Generation.AES_GCM_SIV.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("junk", typeof(TestCaseGeneratorDecrypt))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
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
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
