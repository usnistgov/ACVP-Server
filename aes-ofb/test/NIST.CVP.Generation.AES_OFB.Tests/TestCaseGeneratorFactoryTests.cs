using System;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", "something", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", "kat", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", "MCT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("Junk", "", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                TestType = testType
            };

            var subject = new TestCaseGeneratorFactory(null, null, null);
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
                TestType = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(null, null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
