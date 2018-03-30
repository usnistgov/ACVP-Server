using System;
using System.Collections.Generic;
using System.Text;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.Tests
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
            TestGroup testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            Assert.Throws(typeof(ArgumentException), () => new TestCaseGeneratorKnownAnswer(testGroup, AlgoMode.TDES_CFB1));

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
            TestGroup testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup, AlgoMode.TDES_CFB1);
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
            TestGroup testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup, AlgoMode.TDES_CFB1);
            List<TestCaseGenerateResponse> results = new EditableList<TestCaseGenerateResponse>();
            for (int i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(subject.Generate(testGroup, false));
            }
            Assert.AreEqual(count, results.Count);
        }
    }
}
