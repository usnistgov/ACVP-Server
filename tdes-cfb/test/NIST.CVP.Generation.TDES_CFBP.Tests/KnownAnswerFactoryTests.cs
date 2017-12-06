using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class KnownAnswerFactoryTests
    {


        [Test]
        [TestCase(null, "Decrypt", "tdes-cfbp1")]
        [TestCase("", "Decrypt", "tdes-cfbp1")]
        [TestCase("permutation", "", "tdes-cfbp1")]
        [TestCase("SubstitutiontablE", null, "tdes-cfbp1")]
        public void ShouldReturnEmptyListIfNoTestTypeSupplied(string testType, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        [TestCase("fredo", "decrypt", "tdes-cfbp1")]
        [TestCase("Julie", "decrypt", "tdes-cfbp1")]
        [TestCase("permutation", "dodo", "tdes-cfbp1")]
        [TestCase("SubstitutiontablE", "dreamweaver", "tdes-cfbp1")]
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
        [TestCase("permutation", "decrypt", "tdes-cfbp1")]
        [TestCase("SubstitutiontablE", "encrypt", "tdes-cfbp1")]
        [TestCase("VariableTExt", "decrypt", "tdes-cfbp1")]
        [TestCase("VariableKey", "encrypt", "tdes-cfbp1")]

        public void ShouldReturnFilledList(string testType, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);
        }

        [Test]
        //[TestCase("InversePermutation", 64, "decrypt")]
        [TestCase("Permutation", 64, "decrypt", "tdes-cfbp1")]
        [TestCase("VariableKey", 112, "decrypt", "tdes-cfbp1")]
        [TestCase("VariableText", 128, "encrypt", "tdes-cfbp1")]
        [TestCase("SubstitutionTable", 38, "encrypt", "tdes-cfbp1")]

        public void ShouldReturnExpectedListCount(string testType, int count, string direction, string algorithm)
        {
            var subject = new KnownAnswerTestFactory();
            var result = subject.GetKATTestCases(testType, direction, algorithm);
            Assume.That(result != null);
            Assert.AreEqual(count, result.Count);
        }

    }
}
