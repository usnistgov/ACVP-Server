using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
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
        //[TestCase("InversePermutation", "decrypt")]
        //[TestCase("Inversepermutation", "DecrYpt")]
        [TestCase("permutation", "decrypt")]
        [TestCase("SubstitutiontablE", "encrypt")]
        [TestCase("VariableTExt", "decrypt")]
        [TestCase("VariableKey", "encrypt")]

        public void ShouldReturnFilledList(string testType, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);
        }

        [Test]
        //[TestCase("InversePermutation", 64, "decrypt")]
        [TestCase("Permutation", 64, "decrypt")]
        [TestCase("VariableKey", 112, "decrypt")]
        [TestCase("VariableText", 128, "encrypt")]
        [TestCase("SubstitutionTable", 38, "encrypt")]

        public void ShouldReturnExpectedListCount(string testType, int count, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assume.That(result != null);
            Assert.AreEqual(count, result.Count);
        }

        [Test]
        //[TestCase("InversePermutation", 0, "8000000000000000", "decrypt")]
        //[TestCase("InversePermutation", 63, "0000000000000001", "decrypt")]
        [TestCase("Permutation", 0, "88d55e54f54c97b488d55e54f54c97b488d55e54f54c97b4", "encrypt")]
        [TestCase("Permutation", 7, "c9f00ffc74079067c9f00ffc74079067c9f00ffc74079067", "decrypt")]
        [TestCase("VariableKey", 0, "95a8d72813daa94d95a8d72813daa94d95a8d72813daa94d", "encrypt")]
        [TestCase("VariableKey", 10, "dfdd3cc64dae1642dfdd3cc64dae1642dfdd3cc64dae1642", "decrypt")]
        [TestCase("VariableText", 0, "95f8a5e5dd31d90095f8a5e5dd31d90095f8a5e5dd31d900", "encrypt")]
        [TestCase("VariableText", 4, "20b9e767b2fb145620b9e767b2fb145620b9e767b2fb1456", "encrypt")]
        [TestCase("SubstitutionTable", 0, "690f5b0d9a26939b690f5b0d9a26939b690f5b0d9a26939b", "encrypt")]
        [TestCase("SubstitutionTable", 14, "6fbf1cafcffd05566fbf1cafcffd05566fbf1cafcffd0556", "encrypt")]
        public void ShouldReturnExpectedElement(string testType, int elementId, string expectedCipherHex, string direction)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction);
            Assume.That(result != null);
            Assume.That(result.Count > elementId);
            var testCase = result[elementId];
            Assert.AreEqual(expectedCipherHex.ToUpper(), testCase.CipherText.ToHex());
        }

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
