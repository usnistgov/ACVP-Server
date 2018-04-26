using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldHaveOneForNumberOfTestCases()
        {
            var subject = new TestCaseGeneratorNull();
            Assert.AreEqual(1, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldReturnErrorForInitialGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = subject.Generate(new TestGroup(), false);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorForRedoGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = subject.Generate(new TestGroup(), new TestCase());
            Assert.IsFalse(result.Success);
        }
    }
}
