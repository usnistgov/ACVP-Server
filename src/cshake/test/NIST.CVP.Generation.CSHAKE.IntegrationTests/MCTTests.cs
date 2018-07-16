using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CSHAKE.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTTests
    {
        [Test]
        [TestCase(128, "f07addd0109137bdae25f64eeb277f7f", "9633c01f6c090167e0fcfbd81637a03ee7a5c2be42fb2e1b81c661e6ed481f100e49a079f4c8a00077760c08f65643d3602e58fb9c114cc515d7a87fa520c90e99446abb816e9e0ebfc0e5dca951fda02536ded1e7fc1adbaa1647237d80c379fd83bfea6de2ad0ba1310fb670a38f4b73536148597f321c95ea35f5753e41056e1cd224a084b0a2768cc4dcffe9667ef221f79c3599ca75013da7961f6626273606164bbf2cf4")]
        [TestCase(256, "c34dff9ee4b0238f67ce4daf94db645276db2588a5743361097c595cf31175a0", "905a5e3efdf097fde7108e2a40027e4b8f8018d90b7e67ce655a8ae4cb857c78e34ec1c1f60ec2c6ef2cfaf1c74560e8d9bc56c5e14db62f0def9141c8262fed70a5c0ee81d7346aff4a83f41cbdc05f01ec26ab812206f4ef8466bf869b7e88904e289d398c1980d71c397b7acaa559c3d11d267a1ffb5e24e6cb4a9c2e0a1b7aaf74d6cb9ff825e2d7cde85491a87cff9b458b9827adabe9bd192c6efa39408ecde6cce7712cfe929ffe1e46f1479749a9eb3060741d85854ee8e546d3d05beaf8abef0cc71d560db5c4b8424dae1e5c62c89b7d13dad2c4de54298f16ebaa5a5928c6aa9de44438248ff9f8762d5ba1986dd7646bff22d3bbb8c71d8e26688de568ad00b8547c7e0d8023fb0bca0c6c3982c8629025f869cd2ef55d9aac58f3f5e0977f48a30c2dd04e6280103cbaec0a8fd274d42195146257d9a64e834fd6113e632ab913590adc623f612b377127fdfb2cbf3537c95337097d56701ebd36b46643ac3f39c15823c980d6d542803cc9d6bb0d3652473c96782de65132b5fd464773293ae68b47ce76fdbd69035cc1074a7f4734e504d6976c3e4eb77fca7e931a70a909edf2e518f9ca0d5dcae4ac051227")]
        public void ShouldMonteCarloTestCSHAKEForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new CSHAKE_MCT(new Crypto.CSHAKE.CSHAKE());
            var messageBitString = new BitString(message);
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction
            {
                Capacity = digestSize * 2,
                DigestSize = digestSize,
                FunctionName = "",
                Customization = ""
            };

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 256, 4096));
            var result = subject.MCTHash(hashFunction, messageBitString, domain, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);

            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex());
        }
    }
}
