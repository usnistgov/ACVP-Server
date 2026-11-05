using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.BLAKE2
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftTests
    {
        [Test]
        public async Task ShouldPassWhenDigestMatches()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorAft(testCase);

            var result = await subject.ValidateAsync(testCase);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailWhenDigestDoesNotMatch()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.Digest = new BitString("BEEFFACE");
            var subject = new TestCaseValidatorAft(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason, Does.Contain("Digest"));
        }

        private static TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Message = new BitString("00"),
                Digest = new BitString("786A02F742015903")
            };
        }
    }
}
