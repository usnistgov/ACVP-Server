using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("Aft", typeof(TestCaseGeneratorAft))]
        [TestCase("AFT", typeof(TestCaseGeneratorAft))]
        [TestCase("KaT", typeof(TestCaseGeneratorKat))]
        [TestCase("GDT", typeof(TestCaseGeneratorGdt))]
        [TestCase("gdt", typeof(TestCaseGeneratorGdt))]
        [TestCase("null", typeof(TestCaseGeneratorNull))]
        [TestCase("not relevant", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Modulo = 2048,
                PrimeTest = PrimeTestFips186_4Modes.TblC2
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
