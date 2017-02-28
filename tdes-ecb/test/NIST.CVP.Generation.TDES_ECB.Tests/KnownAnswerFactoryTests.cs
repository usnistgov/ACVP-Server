using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class KnownAnswerFactoryTests
    {


        [Test]
        [TestCase(null, "Decrypt")]
        [TestCase("", "Decrypt")]
        [TestCase("permutation", "")]
        [TestCase("SubstitutiontablE", null)]
        public void ShouldReturnEmptyListIfNoTestTypeSupplied(string testType, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("fredo", "decrypt")]
        [TestCase("Julie", "decrypt")]
        [TestCase("permutation", "dodo")]
        [TestCase("SubstitutiontablE", "dreamweaver")]
        public void ShouldReturnEmptyListIfBadTestTypeSupplied(string testType, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("InversePermutation", "decrypt")]
        [TestCase("Inversepermutation", "DecrYpt")]
        [TestCase("permutation", "DECRYPT")]
        [TestCase("SubstitutiontablE", "encrypt")]
        [TestCase("VariableTExt", "ENcryPt")]
        [TestCase("VariableKey", "ENCRYPT")]

        public void ShouldReturnFilledList(string testType, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);
        }

        [Test]
        [TestCase("InversePermutation", 64, "decrypt")]
        [TestCase("Permutation", 32, "decrypt")]
        [TestCase("VariableKey", 56, "decrypt")]
        [TestCase("VariableText", 64, "encrypt")]
        [TestCase("SubstitutionTable", 19, "encrypt")]

        public void ShouldReturnExpectedListCount(string testType, int count, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assume.That(result != null);
            Assert.AreEqual(count, result.Count);
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
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assume.That(result != null);
            Assume.That(result.Count > elementId);
            var testCase = result[elementId];
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }

        [Test]
        public void ShouldReturnSeparateEncryptDecryptTestCases()
        {
            var subject = new KnownAnswerTestFactory();
            var result1 = subject.GetKATTestCases("InversePermutation", "decrypt");
            var result2 = subject.GetKATTestCases("InversePermutation", "encrypt");
            Assert.AreNotEqual(result1, result2);
        }
    }
}
