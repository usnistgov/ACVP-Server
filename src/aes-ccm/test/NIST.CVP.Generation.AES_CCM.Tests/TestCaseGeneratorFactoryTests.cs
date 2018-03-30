using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Encrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("ENcrypt", typeof(TestCaseGeneratorEncrypt))]
        [TestCase("Decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("Junk", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypto", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
