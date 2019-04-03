using NIST.CVP.Common;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        [Test]
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFBP64_v1_0)]
        public void ShouldThrowIfInvalidTestTypeOrDirection(string testType, string direction, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = algoMode,
                TestType = testType,
                Function = direction
            };

            Assert.Throws(typeof(ArgumentException), () => new TestCaseGeneratorKat(testType, algoMode));
        }

        [Test]
        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFBP1_v1_0)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFBP1_v1_0)]

        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFBP8_v1_0)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFBP8_v1_0)]

        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFBP64_v1_0)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFBP64_v1_0)]
        public async Task ShouldReturnKat(string testType, string direction, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = algoMode,
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKat(testType, algoMode);
            var result = await subject.GenerateAsync(testGroup, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
    }
}
