using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase(KeyGenModes.B32, "not relevant", typeof(TestCaseGeneratorAFT_B32))]
        [TestCase(KeyGenModes.B33, "not relevant", typeof(TestCaseGeneratorGDT_B33))]
        [TestCase(KeyGenModes.B33, "KaT", typeof(KnownAnswerTestCaseGeneratorB33))]
        [TestCase(KeyGenModes.B34, "not relevant", typeof(TestCaseGeneratorAFT_B34))]
        [TestCase(KeyGenModes.B35, "not relevant", typeof(TestCaseGeneratorAFT_B35))]
        [TestCase(KeyGenModes.B36, "not relevant", typeof(TestCaseGeneratorAFT_B36))]
        public void ShouldReturnProperGenerator(KeyGenModes mode, string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Mode = mode,
                TestType = testType,
                Modulo = 2048,
                PrimeTest = PrimeTestModes.C2
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }
    }
}
