using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not relevant", 128, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 128, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", 128, "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", 128, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", 128, "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", 128, "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", 128, "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", 128, "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("ENcrypt", 128, "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Decrypt", 128, "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", 128, "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", 128, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 128, "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 192, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 192, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", 192, "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", 192, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", 192, "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", 192, "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", 192, "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", 192, "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("ENcrypt", 192, "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Decrypt", 192, "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", 192, "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", 192, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 192, "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 256, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 256, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", 256, "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", 256, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", 256, "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", 256, "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", 256, "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", 256, "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("ENcrypt", 256, "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Decrypt", 256, "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", 256, "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", 256, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 256, "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, int keySize, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                KeyLength = keySize,
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
            var subject = new TestCaseGeneratorFactory(null, null, null);
            var generator = subject.GetCaseGenerator(new TestGroup()
            {
                Function = string.Empty,
                TestType = string.Empty
            });
            Assert.IsNotNull(generator);
        }
    }
}
