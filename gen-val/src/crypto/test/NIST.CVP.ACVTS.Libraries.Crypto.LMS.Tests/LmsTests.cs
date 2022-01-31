using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests
{
    [Ignore("Replacing with native implementation")]
    public class LmsTests
    {
        [Test]
        [TestCase(LmsType.LMS_SHA256_M32_H5, LmotsType.LMOTS_SHA256_N32_W1)]
        [TestCase(LmsType.LMS_SHA256_M32_H5, LmotsType.LMOTS_SHA256_N32_W2)]
        [TestCase(LmsType.LMS_SHA256_M32_H5, LmotsType.LMOTS_SHA256_N32_W4)]
        [TestCase(LmsType.LMS_SHA256_M32_H5, LmotsType.LMOTS_SHA256_N32_W8)]
        [TestCase(LmsType.LMS_SHA256_M32_H10, LmotsType.LMOTS_SHA256_N32_W1)]
        [TestCase(LmsType.LMS_SHA256_M32_H10, LmotsType.LMOTS_SHA256_N32_W2)]
        [TestCase(LmsType.LMS_SHA256_M32_H10, LmotsType.LMOTS_SHA256_N32_W4)]
        [TestCase(LmsType.LMS_SHA256_M32_H10, LmotsType.LMOTS_SHA256_N32_W8)]
        [TestCase(LmsType.LMS_SHA256_M32_H15, LmotsType.LMOTS_SHA256_N32_W1)]
        [TestCase(LmsType.LMS_SHA256_M32_H15, LmotsType.LMOTS_SHA256_N32_W2)]
        [TestCase(LmsType.LMS_SHA256_M32_H15, LmotsType.LMOTS_SHA256_N32_W4)]
        [TestCase(LmsType.LMS_SHA256_M32_H15, LmotsType.LMOTS_SHA256_N32_W8)]
        public async Task ShouldVerifySignatureCorrectlyRandomlyGenerated(LmsType lmsType, LmotsType lmotsType)
        {
            var msg = new BitString("7ab290ec2c6649a14abcdef093783242");

            var lms = new Lms(lmsType, lmotsType);

            var keyPair = await lms.GenerateLmsKeyPairAsync();

            var sig = lms.GenerateLmsSignature(msg, keyPair.PrivateKey);

            var verify = lms.VerifyLmsSignature(msg, keyPair.PublicKey, sig);

            Assert.That(verify.Success);
        }
    }
}
