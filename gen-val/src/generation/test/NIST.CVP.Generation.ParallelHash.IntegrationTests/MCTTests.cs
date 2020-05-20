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
        [TestCase(128, "40058A9D25FEE7FC1A27C76F20241F26", "19AB82A22C3A3462D023E677E947245DC1D86F5B0C69E7A61F177DEEC934379077835D2CFE30C12F11449423594F036DA1D21154D534C8BE9502936B5E94B42AD94CCCC4A5EC8B284D19DE4F26DAF6A3EE506B876C8E36B4ECE10285B4097287091B7028B6ECDA65C0EC0CA548852F4B4C31146ADF5A562358F91329B07503528E66D754D76B5693A78EFCF8A64DBA39")]
        [TestCase(256, "93579F4B946D00BA41BF79010D5C3F92F88965451D435E31682EF6BD1B0757E9", "661FEBA7B057018308137BDF2FA26FA2D12441AF7EC5195B2088C2D7120E3F77D5C4BDAE379838F64D491AA7BAA7D7B888E41115BD35D6")]
        public void ShouldMonteCarloTestParallelHashForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new ParallelHash_MCT(new Crypto.ParallelHash.ParallelHash());
            var messageBitString = new BitString(message);
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction(digestSize, digestSize * 2, false);

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 256, 4096));
            
            var blockSizeDomain = new MathDomain();
            blockSizeDomain.AddSegment(new RangeDomainSegment(null, 1, 128));
            var result = subject.MCTHash(hashFunction, messageBitString, domain, blockSizeDomain, false, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);
            
            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            //Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex(), resultDigest.ToHex());
        }
    }
}
