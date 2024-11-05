using System;
using NIST.CVP.ACVTS.Libraries.Generation.AES_GCM_SIV.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.GCM_SIV
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
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
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
            Assert.That(generator, Is.Not.Null);
        }
    }
}
