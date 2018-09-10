using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("permutationn")]
        [TestCase("SubstitutiontablEe")]
        [TestCase("fredo")]
        [TestCase("Julie")]
        [TestCase("permutation2")]
        [TestCase("SubbstitutiontablE")]
        public void ShouldThrowIfInvalidTestType(string testType)
        {
            Assert.Throws(typeof(ArgumentException), () => new TestCaseGeneratorKat(testType));
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
            TestGroup testGroup = new TestGroup()
            {
                TestType = testType,
                Function = direction
            };

            var subject = new TestCaseGeneratorKat(testType);
            var result = await subject.GenerateAsync(testGroup, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
    }
}
