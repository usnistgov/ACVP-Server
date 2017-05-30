using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.GenVal.Tests
{
    [TestFixture, UnitTest]
    public class KnownAnswerTestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldReturnResponseWithErrorMessage()
        {
            TestGroup testGroup = new TestGroup();

            KnownAnswerTestCaseGeneratorNull subject = new KnownAnswerTestCaseGeneratorNull();
            var result = subject.Generate(testGroup);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
