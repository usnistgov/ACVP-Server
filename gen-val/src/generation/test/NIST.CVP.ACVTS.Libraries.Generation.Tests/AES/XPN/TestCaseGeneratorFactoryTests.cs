using System;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XPN
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("encrypt", "external", "external", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("Encrypt", "external", "extERnal", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("Encrypt", "EXternaL", "external", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("ENcrypt", "External", "external", typeof(TestCaseGeneratorExternalEncrypt))]
        [TestCase("Encrypt", "Internal", "external", typeof(TestCaseGeneratorInternalEncrypt))]
        [TestCase("encrypt", "internal", "external", typeof(TestCaseGeneratorInternalEncrypt))]
        [TestCase("Encrypt", "external", "Internal", typeof(TestCaseGeneratorInternalEncrypt))]
        [TestCase("encrypt", "external", "internal", typeof(TestCaseGeneratorInternalEncrypt))]
        [TestCase("Decrypt", "Internal", "external", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", "internal", "external", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", "external", "external", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("decrypt", "junk", "external", typeof(TestCaseGeneratorDecrypt))]
        [TestCase("Junk", "internal", "external", typeof(TestCaseGeneratorNull))]
        [TestCase("encrypt", "junk", "junk", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string ivGen, string saltGen, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                Function = direction,
                IvGeneration = ivGen,
                SaltGen = saltGen
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
                IvGeneration = string.Empty,
                SaltGen = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
