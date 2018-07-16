using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTTests
    {
        [Test]
        [TestCase(128, "6dc13301be877363018fbbbddd779b7d", "776ba3c77911bf06cb1e841a761a4b1bcae501ea042b8417f9ca30b42047a4f747284d797e7e7308326fa1a4a6881d6598f027c910b807067b187e4860ff319754604b758a2ce2083c08a233d919bc30c8e1ab948666e6e433b0288c357dae0058c21499329e4578f5d5c296fe9f96c364f7fd0c6e347daecb4e168693f6c8451306c7985f31ab75facb00d1e3f15e828e5b303805729f996ca8d745b61bfc08729ce436a4761ed2ab2132916baa7c3b4021912c31b1bc091dd3045e637cd038e84c7a544ac480345bc69c1543c356cc69bd967c08d3df7f6da084310aca086a44267bf85683f9840412119c0cf17ae61b70400e0836e3836e89630e0f92e390bb2676c11f521e01a4e4c0afd11b9bfdfc3f5787e6e61bab3696f01dba60d8fe7cf2dae2e6ea0ceedad682987044943bb42e413726482881a78966f5ecdf07")]
        [TestCase(256, "acb91ac0a42bb1653fdd3b43ce7e8aa259f36a62a77b533aa54ade01d72dffc2", "efacac3d6addccfa6f7845d300d83a45871c80385d756b66aad52b9005b00930999d1c3f740d0d4b2ab73944a4282df47cffb11a864fc9bbadf8539459114dab7a816696573ffeac01a22595463073ce8e4c48429a8159e2b7730075e48ce693867b9e14e8c1efffe156b476bfcf4dda9ea45d354732673f3869ed19028dae3fa116ce044bfc7fdc0858104d25c3a7330995b5e1fffa425b4074ab8d0a427a19fa2caa04c034ece4d508a211fbca09e9695df4e4b36e88738f7ffa835b6fae48fc566bea83ee0354a513363c75d1f52e39e80b9755da6c11e6946b902eed9d118213684f1e0cfdc791f045613b9fbc9dd1cc0c44f97ebb881829c9c7debb954e56c3a2f7b927f7e800db7918034e49fc0ae20093a6af452cde6ce47a60c1678158745e0a48dcaf029066b4d769bd705c1f933a08863ffc83597cc51dd37f2b428221e9e450f962964abf525fce5194eaa00cd50fc56a96f7002830c666facbdb5b3791342424fcae6cf4067f7ea83565d60e6a3d06482c7ee6f12af3b2ec54794aea18c04dc89cc6c2f90107e740c8bef6cb39615a762e7fd4a328f31eae79d1bbcfbf5c7189b50398733aa66b91166f6acb3e7d1dac2a23c2d3f5d58cfc4cee4f")]
        public void ShouldMonteCarloTestParallelHashForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new ParallelHash_MCT(new Crypto.ParallelHash.ParallelHash());
            var messageBitString = new BitString(message);
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction
            {
                Capacity = digestSize * 2,
                DigestSize = digestSize,
                BlockSize = 8,
                Customization = ""
            };

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 256, 2048));
            var result = subject.MCTHash(hashFunction, messageBitString, domain, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);

            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex());
        }
    }
}
