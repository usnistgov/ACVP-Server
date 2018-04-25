using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("Aft", typeof(TestCaseGeneratorAft))]
        [TestCase("AFT", typeof(TestCaseGeneratorAft))]
        [TestCase("KaT", typeof(TestCaseGeneratorKat))]
        [TestCase("GDT", typeof(TestCaseGeneratorAft))]
        [TestCase("gdt", typeof(TestCaseGeneratorAft))]
        [TestCase("null", typeof(TestCaseGeneratorNull))]
        [TestCase("not relevant", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Modulo = 2048,
                PrimeTest = PrimeTestModes.C2
            };

            var subject = new TestCaseGeneratorFactory(null, null, null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
