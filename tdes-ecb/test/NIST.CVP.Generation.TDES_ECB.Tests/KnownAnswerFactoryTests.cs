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
        [TestCase(null)]
        [TestCase("")]
        public void ShouldReturnEmptyListIfNoTestTypeSupplied(string testType)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("fredo")]
        [TestCase("Julie")]
        public void ShouldReturnEmptyListIfBadTestTypeSupplied(string testType)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("InversePermutation")]
        [TestCase("Inversepermutation")]
        [TestCase("permutation")]
        [TestCase("SubstitutiontablE")]
        [TestCase("VariableTExt")]
        [TestCase("VariableKey")]

        public void ShouldReturnFilledList(string testType)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count,0);
        }

        [Test]
        [TestCase("InversePermutation", 64)]
        [TestCase("Permutation", 32)]
        [TestCase("VariableKey", 56)]
        [TestCase("VariableText", 64)]
        [TestCase("SubstitutionTable", 19)]
      
        public void ShouldReturnExpectedListCount(string testType, int count)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType);
            Assume.That(result != null);
            Assert.AreEqual(count, result.Count);
        }

        [Test]
        [TestCase("InversePermutation", 0, "8000000000000000")]
        [TestCase("InversePermutation", 63, "0000000000000001")]
        [TestCase("Permutation", 0, "88d55e54f54c97b4")]
        [TestCase("Permutation", 31, "1aeac39a61f0a464")]
        [TestCase("VariableKey", 0, "95a8d72813daa94d")]
        [TestCase("VariableKey", 55, "869efd7f9f265a09")]
        [TestCase("VariableText", 0, "95f8a5e5dd31d900")]
        [TestCase("VariableText", 63, "166b40b44aba4bd6")]
        [TestCase("SubstitutionTable", 0, "690f5b0d9a26939b")]
        [TestCase("SubstitutionTable", 18, "63fac0d034d9f793")]
        public void ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType);
            Assume.That(result != null);
            Assume.That(result.Count > elementId);
            var testCase = result[elementId];
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }
    }
}
