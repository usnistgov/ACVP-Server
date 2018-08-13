using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_OFB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        [Test]
        [TestCase(null, "Decrypt")]
        [TestCase("", "Decrypt")]
        [TestCase("permutation", "")]
        [TestCase("SubstitutiontablE", null)]
        [TestCase("fredo", "decrypt")]
        [TestCase("Julie", "decrypt")]
        [TestCase("permutation", "dodo")]
        [TestCase("SubstitutiontablE", "dreamweaver")]
        public void ShouldThrowIfInvalidTestTypeOrDirection(string testType, string direction)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Function = direction
            };

            Assert.Throws(typeof(ArgumentException), () => new TestCaseGeneratorKnownAnswer(testGroup));
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
            var testGroup = new TestGroup
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var result = await subject.GenerateAsync(testGroup, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("Permutation", 32, "decrypt")]
        [TestCase("VariableKey", 64, "decrypt")]
        [TestCase("VariableText", 64, "encrypt")]
        [TestCase("SubstitutionTable", 19, "encrypt")]
        public async Task ShouldReturnExpectedListCount(string testType, int count, string direction)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            List<TestCaseGenerateResponse<TestGroup, TestCase>> results = new EditableList<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await subject.GenerateAsync(testGroup, false));
            }
            Assert.AreEqual(count, results.Count);
        }

        [Test]
        [TestCase("Permutation", 0, "88d55e54f54c97b4", "decrypt")]
        [TestCase("Permutation", 31, "1aeac39a61f0a464", "decrypt")]
        [TestCase("VariableKey", 0, "95f8a5e5dd31d900", "decrypt")]
        [TestCase("VariableKey", 55, "dd7c0bbd61fafd54", "decrypt")]
        [TestCase("VariableText", 0, "95f8a5e5dd31d900", "encrypt")]
        [TestCase("VariableText", 63, "166b40b44aba4bd6", "encrypt")]
        [TestCase("SubstitutionTable", 0, "690f5b0d9a26939b", "encrypt")]
        [TestCase("SubstitutionTable", 18, "63fac0d034d9f793", "encrypt")]
        public async Task ShouldReturnExpectedElement(string testType, int elementId, string expected, string direction)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await subject.GenerateAsync(testGroup, false));
            }

            Assume.That(results.Count > elementId);
            var testCase = results[elementId].TestCase;
            Assert.AreEqual(expected.ToUpper(), testCase.PlainText.ToHex());
        }
    }
}
