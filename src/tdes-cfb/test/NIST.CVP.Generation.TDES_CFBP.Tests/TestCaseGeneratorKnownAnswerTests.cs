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
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFBP1)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFBP1)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFBP1)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFBP1)]
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFBP8)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFBP8)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFBP8)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFBP8)]
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFBP64)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFBP64)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFBP64)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFBP64)]
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
        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFBP1)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFBP1)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFBP1)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFBP1)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFBP1)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFBP1)]

        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFBP8)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFBP8)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFBP8)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFBP8)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFBP8)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFBP8)]

        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFBP64)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFBP64)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFBP64)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFBP64)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFBP64)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFBP64)]
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
