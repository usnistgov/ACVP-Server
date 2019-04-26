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
        [TestCase(128, "40058A9D25FEE7FC1A27C76F20241F26", "3A79B82DBF6CD400E34019AE23ADE34EFACDA57FBA486D2A30E16915A6C6796BB961EE9556E27D027A91E050C822305414D10E8630AB1933C6101CD2F525B3B28C46B3A98FAFF6228F576E68A8CE111EE43ED473D9F14EA551404FA597C4555015E2D7F59949557941721C226FDB9EAE03BD708B19438B4F90ED2530CFDF2E0E32221DD064EC057C916CEEE075703AB1C0238E20AE8D53ECAA903ECF2A93B9ADDE16554824CAFFC05C6813A97A0DBA3267FDE48D9C17154ACDF44EC3E3F7CB25012EA3C743D101AAD9776879E2EDFA2C3358195748", TestName = "ParallelHash 128 MCT")]
        [TestCase(256, "93579F4B946D00BA41BF79010D5C3F92F88965451D435E31682EF6BD1B0757E9", "B04559ADD8CF64E60C1520ED11CE3381A5BDC4BE3522404DF8A5C4C20E16DD4D689F049416B440CC92BDA366921FB6E65C0811F57D686676D0656A07473011E5BFDF846C403B382868EBEC56B3F29995EAC23DBC3872AAADCDE987876DD340F62EDB", TestName = "ParallelHash 256 MCT")]
        public void ShouldMonteCarloTestParallelHashForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new ParallelHash_MCT(new Crypto.ParallelHash.ParallelHash());
            var messageBitString = new BitString(message);
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction(digestSize, digestSize * 2, false);

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 256, 4096));
            var result = subject.MCTHash(hashFunction, messageBitString, domain, false, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);

            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex());
        }
    }
}
