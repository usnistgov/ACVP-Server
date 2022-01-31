using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorSigTests
    {
        [Test]
        [TestCase("beefface", "beefface")]
        [TestCase("facebeef", "facebeef")]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodTest(string expected, string supplied)
        {
            var subject = new TestCaseValidatorSig(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase("beefface", "facebeef")]
        [TestCase("facebeef", "beefface")]
        public async Task ShouldRunVerifyMethodAndFailWithBadTest(string expected, string supplied)
        {
            var subject = new TestCaseValidatorSig(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private TestCase GetResultTestCase(string signature)
        {
            return new TestCase
            {
                TestCaseId = 1,
                Signature = new BitString(signature)
            };
        }
    }
}

