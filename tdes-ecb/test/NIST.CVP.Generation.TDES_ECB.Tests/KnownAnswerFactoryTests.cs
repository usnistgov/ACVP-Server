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
        public void ShouldReturnEmptyListIfNoDirectionSupplied(string direction)
        {
            var subject=  new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(direction, "permutation");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ShouldReturnEmptyListIfNoTestTypeSupplied(string testType)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases("encrypt", testType);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("fredo")]
        [TestCase("Julie")]
        public void ShouldReturnEmptyListIfBadTestTypeSupplied(string testType)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases("encrypt", testType);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        [TestCase("Bad")]
        [TestCase("BCrypt")]
        public void ShouldReturnEmptyListIfBadDirectionSupplied(string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(direction, "permutation");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        [TestCase("Encrypt", "InversePermutation")]
        [TestCase("encrypt", "Inversepermutation")]
        [TestCase("encrypt", "permutation")]
        [TestCase("encrypt", "SubstitutiontablE")]
        [TestCase("encrypT", "VariableTExt")]
        [TestCase("encRypt", "VariableKey")]
        public void ShouldReturnFilledList(string direction, string testType)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(direction, testType);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count,0);
        }

        [Test]
        [TestCase("Encrypt", "InversePermutation", 64)]
        [TestCase("Encrypt", "Permutation", 32)]
        [TestCase("Encrypt", "VariableKey", 56)]
        [TestCase("Encrypt", "VariableText", 64)]
        [TestCase("Encrypt", "SubstitutionTable", 19)]
        public void ShouldReturnExpectedListCount(string direction, string testType, int count)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(direction, testType);
            Assume.That(result != null);
            Assert.AreEqual(count, result.Count);
        }

        [Test]
        [TestCase("Encrypt", "InversePermutation", 0, "8000000000000000")]
        [TestCase("Encrypt", "InversePermutation", 63, "0000000000000001")]
        [TestCase("Encrypt", "Permutation", 0, "88d55e54f54c97b4")]
        [TestCase("Encrypt", "Permutation", 31, "1aeac39a61f0a464")]
        [TestCase("Encrypt", "VariableKey", 0, "95a8d72813daa94d")]
        [TestCase("Encrypt", "VariableKey", 55, "869efd7f9f265a09")]
        [TestCase("Encrypt", "VariableText", 0, "95f8a5e5dd31d900")]
        [TestCase("Encrypt", "VariableText", 63, "166b40b44aba4bd6")]
        [TestCase("Encrypt", "SubstitutionTable", 0, "690f5b0d9a26939b")]
        [TestCase("Encrypt", "SubstitutionTable", 18, "63fac0d034d9f793")]
        public void ShouldReturnExpectedElement(string direction, string testType, int elementId, string expectedCipherHex )
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(direction, testType);
            Assume.That(result != null);
            Assume.That(result.Count > elementId);
            var testCase = result[elementId];
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }
    }
}
