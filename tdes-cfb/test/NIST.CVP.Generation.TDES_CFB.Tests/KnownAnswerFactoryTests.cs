using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
    public class KnownAnswerFactoryTests
    {


        [Test]
        [TestCase(null, "Decrypt", "tdes-cfb1")]
        [TestCase("", "Decrypt", "tdes-cfb1")]
        [TestCase("permutation", "", "tdes-cfb1")]
        [TestCase("SubstitutiontablE", null, "tdes-cfb1")]
        public void ShouldReturnEmptyListIfNoTestTypeSupplied(string testType, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("fredo", "decrypt", "tdes-cfb1")]
        [TestCase("Julie", "decrypt", "tdes-cfb1")]
        [TestCase("permutation", "dodo", "tdes-cfb1")]
        [TestCase("SubstitutiontablE", "dreamweaver", "tdes-cfb1")]
        public void ShouldReturnEmptyListIfBadTestTypeSupplied(string testType, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        //[TestCase("InversePermutation", "decrypt")]
        //[TestCase("Inversepermutation", "DecrYpt")]
        [TestCase("permutation", "decrypt", "tdes-cfb1")]
        [TestCase("SubstitutiontablE", "encrypt", "tdes-cfb1")]
        [TestCase("VariableTExt", "decrypt", "tdes-cfb1")]
        [TestCase("VariableKey", "encrypt", "tdes-cfb1")]

        public void ShouldReturnFilledList(string testType, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);
        }

        [Test]
        //[TestCase("InversePermutation", 64, "decrypt")]
        [TestCase("Permutation", 64, "decrypt", "tdes-cfb1")]
        [TestCase("VariableKey", 112, "decrypt", "tdes-cfb1")]
        [TestCase("VariableText", 128, "encrypt", "tdes-cfb1")]
        [TestCase("SubstitutionTable", 38, "encrypt", "tdes-cfb1")]

        public void ShouldReturnExpectedListCount(string testType, int count, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assume.That(result != null);
            Assert.AreEqual(count, result.Count);
        }

        //[Test]
        //[TestCase("InversePermutation", 0, "8000000000000000", "decrypt")]
        //[TestCase("InversePermutation", 63, "0000000000000001", "decrypt")]
        //[TestCase("Permutation", 0, "88d55e54f54c97b4", "decrypt", "tdes-cfb1")]
        //[TestCase("Permutation", 31, "1aeac39a61f0a464", "decrypt", "tdes-cfb1")]
        //[TestCase("VariableKey", 0, "95f8a5e5dd31d900", "decrypt", "tdes-cfb1")]
        //[TestCase("VariableKey", 55, "dd7c0bbd61fafd54", "decrypt", "tdes-cfb1")]
        //[TestCase("VariableText", 0, "95f8a5e5dd31d900", "encrypt", "tdes-cfb1")]
        //[TestCase("VariableText", 63, "166b40b44aba4bd6", "encrypt", "tdes-cfb1")]
        //[TestCase("SubstitutionTable", 0, "690f5b0d9a26939b", "encrypt", "tdes-cfb1")]
        //[TestCase("SubstitutionTable", 18, "63fac0d034d9f793", "encrypt", "tdes-cfb1")]
        //public void ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex, string direction, string algorithm)
        //{
        //    var subject = new KnownAnswerTestFactory();
        //    var result = subject.GetKATTestCases(testType, direction, algorithm);
        //    Assume.That(result != null);
        //    Assume.That(result.Count > elementId);
        //    var testCase = result[elementId];
        //    Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.PlainText.ToHex());
        //}

        //[Test]
        //public void ShouldReturnSeparateEncryptDecryptTestCases()
        //{
        //    var subject = new KnownAnswerTestFactory();
        //    var result1 = subject.GetKATTestCases("InversePermutation", "decrypt");
        //    var result2 = subject.GetKATTestCases("InversePermutation", "encrypt");
        //    Assert.AreNotEqual(result1, result2);
        //}
    }
}
