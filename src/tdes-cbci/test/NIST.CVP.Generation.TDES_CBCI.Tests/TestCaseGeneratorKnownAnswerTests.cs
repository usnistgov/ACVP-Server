using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
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
            TestGroup testGroup = new TestGroup()
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
        [TestCase("InversePermutation", 128, "decrypt")]
        [TestCase("Permutation", 64, "decrypt")]
        [TestCase("VariableKey", 112, "decrypt")]
        [TestCase("VariableText", 128, "encrypt")]
        [TestCase("SubstitutionTable", 38, "encrypt")]

        public async Task ShouldReturnExpectedListCount(string testType, int count, string direction)
        {
            TestGroup testGroup = new TestGroup()
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
        [TestCase("Permutation", 0, "88d55e54f54c97b488d55e54f54c97b488d55e54f54c97b4", "encrypt")]
        [TestCase("Permutation", 7, "c9f00ffc74079067c9f00ffc74079067c9f00ffc74079067", "decrypt")]
        [TestCase("VariableKey", 0, "95a8d72813daa94d95a8d72813daa94d95a8d72813daa94d", "encrypt")]
        [TestCase("VariableKey", 10, "dfdd3cc64dae1642dfdd3cc64dae1642dfdd3cc64dae1642", "decrypt")]
        [TestCase("VariableText", 0, "95f8a5e5dd31d90095f8a5e5dd31d90095f8a5e5dd31d900", "encrypt")]
        [TestCase("VariableText", 4, "20b9e767b2fb145620b9e767b2fb145620b9e767b2fb1456", "encrypt")]
        [TestCase("SubstitutionTable", 0, "690f5b0d9a26939b690f5b0d9a26939b690f5b0d9a26939b", "encrypt")]
        [TestCase("SubstitutionTable", 14, "6fbf1cafcffd05566fbf1cafcffd05566fbf1cafcffd0556", "encrypt")]
        public async Task ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex, string direction)
        {
            TestGroup testGroup = new TestGroup()
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

            Assume.That(results.Count > elementId);
            var testCase = results[elementId].TestCase;
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }
       
    }
}
