using System;
using NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.GCM
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "external", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("Encrypt", "external", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("Encrypt", "EXternaL", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("ENcrypt", "External", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("Encrypt", "Internal", typeof(TestCaseGeneratorInternalEncrypt))]
        [TestCase("encrypt", "internal", typeof(TestCaseGeneratorInternalEncrypt))]
        [TestCase("Decrypt", "Internal", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", "internal", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", "external", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", "junk", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("Junk", "internal", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "junk", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string ivGen, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                IvGeneration = ivGen
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = string.Empty,
                IvGeneration = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
