﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
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
            var testGroup = new TestGroup()
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
            var testGroup = new TestGroup()
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
        [TestCase("InversePermutation", 64, "decrypt")]
        [TestCase("Permutation", 32, "decrypt")]
        [TestCase("VariableKey", 56, "decrypt")]
        [TestCase("VariableText", 64, "encrypt")]
        [TestCase("SubstitutionTable", 19, "encrypt")]
        public void ShouldReturnExpectedListCount(string testType, int count, string direction)
        {
            var testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var results = new EditableList<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(subject.Generate(testGroup, false));
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
        public void ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex, string direction)
        {
            var testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKnownAnswer(testGroup);
            var results = new EditableList<TestCaseGenerateResponse<TestGroup, TestCase>>();
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