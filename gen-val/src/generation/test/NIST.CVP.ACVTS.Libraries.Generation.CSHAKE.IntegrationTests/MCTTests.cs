using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
//using System;

namespace NIST.CVP.ACVTS.Libraries.Generation.CSHAKE.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTTests
    {
        [Test]
        [TestCase(128, "A32CC00B6229FB67BCE8306D2BED6089", "C5D9E3689811F495EA6109E3BA182F310C476CCC086B20AE9B057616A8E5374C888DDD1540E407726624704E78D864A72432AAC0826CE98527835A066CD96C5B2633F7434625DFB870511CDA5E4A653D2CE023666B94B79BA475EA665062798DA3BF0EC8F6A3BB26BEA0F9776304483DAA68F8C3ED4263D47EE63335AF22AF919AE725EEDEBDEBAF1F268B3F77880C70597288FFAC8D7D97B9188D0992863C53DCC8BDE584AB102C0E2A8EA025E07BC9F68EE5C34DCCFE4FA308B648B6D8977DA179BEB28C850A673DBB520CC46D071AD739A5B29562B60CA95B06E1AE2A5A9524EA6B6F3ADF2E393D6EA74B2DC40205527C7D1B55CA30339CEFD2CA57EE5E84E7418491C2FBBB55AEFC526859116716CEFE7A92D76388B18C6BAA2D9C3077EC4B3314A104CAB61924A58C9382D7B4930DC278D36EB5885838DF3C724007DCE833", TestName = "CSHAKE 128 MCT")]
        [TestCase(256, "DE50427C2F080E46494402089412F28953CE79A33B374D22F544A27745F4191B", "E73BD26E5AC00C7CF07CA10F36E20D1122564FF63A408EF0FE7C9512F8196A2525469092948CCACE725C", TestName = "CSHAKE 256 MCT")]
        public void ShouldMonteCarloTestCSHAKEForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new CSHAKE_MCT(new Crypto.CSHAKE.CSHAKE());
            var messageBitString = new BitString(message);
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction(digestSize, digestSize * 2);

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 256, 4096, 8));    // This is how they were generated
            var result = subject.MCTHash(hashFunction, messageBitString, domain, false, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);

            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            //            Console.WriteLine($"Should be: {digestBitString.ToHex()}");
            //           Console.WriteLine($"Is       : {resultDigest.ToHex()}");
            Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex());
        }
    }
}
