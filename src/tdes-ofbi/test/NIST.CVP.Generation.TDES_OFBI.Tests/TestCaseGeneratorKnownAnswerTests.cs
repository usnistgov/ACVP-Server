using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_OFBI.Tests
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
        public void ShouldReturnKat(string testType, string direction)
        {
            var testGroup = new TestGroup
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var result = subject.Generate(testGroup, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("InversePermutation", 128, "decrypt")]
        [TestCase("Permutation", 64, "decrypt")]
        [TestCase("VariableKey", 112, "decrypt")]
        [TestCase("VariableText", 128, "encrypt")]
        [TestCase("SubstitutionTable", 38, "encrypt")]
        public void ShouldReturnExpectedListCount(string testType, int count, string direction)
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
                results.Add(subject.Generate(testGroup, false));
            }

            Assert.AreEqual(count, results.Count);
        }

        [Test]
        [TestCase("Permutation", 0, "88d55e54f54c97b423c25ab3e19b6b94e5b490db69b0f2ec", "encrypt")]
        [TestCase("Permutation", 7, "c9f00ffc74079067d59da3b97fa77d573ad69f58d64555fd", "decrypt")]
        [TestCase("VariableKey", 0, "95a8d72813daa94db8bc8dbc0b24cfa91e08a515c11e0de1", "encrypt")]
        [TestCase("VariableKey", 10, "dfdd3cc64dae1642bb34b6ec92447bdc99547b8b947e8c44", "decrypt")]
        [TestCase("VariableText", 0, "95f8a5e5dd31d900f7552ab6cb21e2bc5a48d3de869557fd", "encrypt")]
        [TestCase("VariableText", 4, "20b9e767b2fb1456c39193d42381b3134f1b8036d441af95", "encrypt")]
        [TestCase("SubstitutionTable", 0, "690f5b0d9a26939b97fc1b9381f05ffae90a658ca212b240", "encrypt")]
        [TestCase("SubstitutionTable", 14, "6fbf1cafcffd05560aa3768ad4358b6c68b40c29c2238233", "encrypt")]
        public void ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex, string direction)
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
                results.Add(subject.Generate(testGroup, false));
            }

            Assume.That(results.Count > elementId);
            var testCase = results[elementId].TestCase;
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }
    }
}
