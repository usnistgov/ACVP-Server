using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        [Test]
        [TestCase(null, "Decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("permutation", "", AlgoMode.TDES_CFB1)]
        [TestCase("SubstitutiontablE", null, AlgoMode.TDES_CFB1)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("permutation", "dodo", AlgoMode.TDES_CFB1)]
        [TestCase("SubstitutiontablE", "dreamweaver", AlgoMode.TDES_CFB1)]

        [TestCase(null, "Decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("permutation", "", AlgoMode.TDES_CFB8)]
        [TestCase("SubstitutiontablE", null, AlgoMode.TDES_CFB8)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("permutation", "dodo", AlgoMode.TDES_CFB8)]
        [TestCase("SubstitutiontablE", "dreamweaver", AlgoMode.TDES_CFB8)]

        [TestCase(null, "Decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("", "Decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("permutation", "", AlgoMode.TDES_CFB64)]
        [TestCase("SubstitutiontablE", null, AlgoMode.TDES_CFB64)]
        [TestCase("fredo", "decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("Julie", "decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("permutation", "dodo", AlgoMode.TDES_CFB64)]
        [TestCase("SubstitutiontablE", "dreamweaver", AlgoMode.TDES_CFB64)]
        public void ShouldThrowIfInvalidTestTypeOrDirection(string testType, string direction, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = algoMode,
                TestType = testType,
                Function = direction
            };

            Assert.Throws(typeof(ArgumentException), () => new TestCaseGeneratorKnownAnswer(testGroup));

        }

        [Test]
        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFB1)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFB1)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFB1)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFB1)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFB1)]

        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFB8)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFB8)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFB8)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFB8)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFB8)]

        [TestCase("InversePermutation", "decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("Inversepermutation", "DecrYpt", AlgoMode.TDES_CFB64)]
        [TestCase("permutation", "DECRYPT", AlgoMode.TDES_CFB64)]
        [TestCase("SubstitutiontablE", "encrypt", AlgoMode.TDES_CFB64)]
        [TestCase("VariableTExt", "ENcryPt", AlgoMode.TDES_CFB64)]
        [TestCase("VariableKey", "ENCRYPT", AlgoMode.TDES_CFB64)]
        public async Task ShouldReturnKat(string testType, string direction, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = algoMode,
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var result = await subject.GenerateAsync(testGroup, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("InversePermutation", 128, "decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("Permutation", 64, "decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("VariableKey", 112, "decrypt", AlgoMode.TDES_CFB1)]
        [TestCase("VariableText", 128, "encrypt", AlgoMode.TDES_CFB1)]
        [TestCase("SubstitutionTable", 38, "encrypt", AlgoMode.TDES_CFB1)]

        [TestCase("InversePermutation", 128, "decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("Permutation", 64, "decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("VariableKey", 112, "decrypt", AlgoMode.TDES_CFB8)]
        [TestCase("VariableText", 128, "encrypt", AlgoMode.TDES_CFB8)]
        [TestCase("SubstitutionTable", 38, "encrypt", AlgoMode.TDES_CFB8)]

        [TestCase("InversePermutation", 128, "decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("Permutation", 64, "decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("VariableKey", 112, "decrypt", AlgoMode.TDES_CFB64)]
        [TestCase("VariableText", 128, "encrypt", AlgoMode.TDES_CFB64)]
        [TestCase("SubstitutionTable", 38, "encrypt", AlgoMode.TDES_CFB64)]
        public async Task ShouldReturnExpectedListCount(string testType, int count, string direction, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = algoMode,
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await subject.GenerateAsync(testGroup, false));
            }
            Assert.AreEqual(count, results.Count);
        }
    }
}
