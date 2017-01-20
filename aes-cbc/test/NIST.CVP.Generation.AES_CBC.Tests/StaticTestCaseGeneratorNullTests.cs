using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.Tests
{
    [TestFixture]
    public class StaticTestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldReturnResponseWithErrorMessage()
        {
            TestGroup testGroup = new TestGroup();

            StaticTestCaseGeneratorNull subject = new StaticTestCaseGeneratorNull();
            var result = subject.Generate(testGroup);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
