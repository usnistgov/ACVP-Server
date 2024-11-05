using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XPN
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public async Task ShouldReturnErrorForInitialGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = await subject.GenerateAsync(new TestGroup(), false);
            Assert.That(result.Success, Is.False);
        }
    }
}
