using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("permutationn")]
        [TestCase("SubstitutiontablEe")]
        [TestCase("fredo")]
        [TestCase("Julie")]
        [TestCase("permutation2")]
        [TestCase("SubbstitutiontablE")]
        public void ShouldThrowIfInvalidTestType(string testType)
        {
            Assert.Throws(typeof(ArgumentException), () => new TestCaseGeneratorKat(testType));  
        }

        [Test]
        [TestCase("InversePermutation", "decrypt")]
        [TestCase("Inversepermutation", "DecrYpt")]
        [TestCase("permutation", "DECRYPT")]
        [TestCase("SubstitutiontablE", "encrypt")]
        [TestCase("VariableTExt", "ENcryPt")]
        [TestCase("VariableKey", "ENCRYPT")]
        public async Task ShouldReturnKat(string testType, string direction)
        {
            var testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKat(testType);
            var result = await subject.GenerateAsync(testGroup, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("InversePermutation", 64, "decrypt")]
        [TestCase("Permutation", 32, "decrypt")]
        [TestCase("VariableKey", 56, "decrypt")]
        [TestCase("VariableText", 64, "encrypt")]
        [TestCase("SubstitutionTable", 19, "encrypt")]
        public async Task ShouldReturnExpectedListCount(string testType, int count, string direction)
        {
            var testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKat(testType);
            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await subject.GenerateAsync(testGroup, false));
            }

            Assert.AreEqual(count, results.Count);
        }

        [Test]
        [TestCase("InversePermutation", 0, "8000000000000000", "decrypt")]
        [TestCase("InversePermutation", 63, "0000000000000001", "decrypt")]
        [TestCase("Permutation", 0, "88d55e54f54c97b4", "decrypt")]
        [TestCase("Permutation", 31, "1aeac39a61f0a464", "decrypt")]
        [TestCase("VariableKey", 0, "95a8d72813daa94d", "decrypt")]
        [TestCase("VariableKey", 55, "869efd7f9f265a09", "decrypt")]
        [TestCase("VariableText", 0, "95f8a5e5dd31d900", "encrypt")]
        [TestCase("VariableText", 63, "166b40b44aba4bd6", "encrypt")]
        [TestCase("SubstitutionTable", 0, "690f5b0d9a26939b", "encrypt")]
        [TestCase("SubstitutionTable", 18, "63fac0d034d9f793", "encrypt")]
        public async Task ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex, string direction)
        {
            var testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKat(testType);
            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await subject.GenerateAsync(testGroup, false));
            }
            
            Assume.That(results.Count > elementId);
            var testCase = results[elementId].TestCase;
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }
    }
}
