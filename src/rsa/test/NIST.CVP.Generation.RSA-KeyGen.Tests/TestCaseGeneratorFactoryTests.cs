using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;

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
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
