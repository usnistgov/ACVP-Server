using System;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v1_0
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
            Assert.That(generator != null);
            Assert.That(generator, Is.InstanceOf(expectedType));
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
            Assert.That(generator, Is.Not.Null);
        }
    }
}
