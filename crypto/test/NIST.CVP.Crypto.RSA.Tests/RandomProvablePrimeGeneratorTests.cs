using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    [TestFixture]
    public class RandomProvablePrimeGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD")]
        [TestCase(2048, "03", "ABCD")]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed)
        {
            var subject = new RandomProvablePrimeGenerator();
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(2048, "0100000001", "e5f707e49c4e7cc8fb202b5cd957963713f1c4726677c09b6a7f5dfe", "ff03b1a74827c746db83d2eaff00067622f545b62584321256e62b01509f10962f9c5c8fd0b7f5184a9ce8e81f439df47dda14563dd55a221799d2aa57ed2713271678a5a0b8b40a84ad13d5b6e6599e6467c670109cf1f45ccfed8f75ea3b814548ab294626fe4d14ff764dd8b091f11a0943a2dd2b983b0df02f4c4d00b413", "dacaabc1dc57faa9fd6a4274c4d588765a1d3311c22e57d8101431b07eb3ddcb05d77d9a742ac2322fe6a063bd1e05acb13b0fe91c70115c2b1eee1155e072527011a5f849de7072a1ce8e6b71db525fbcda7a89aaed46d27aca5eaeaf35a26270a4a833c5cda681ffd49baa0f610bad100cdf47cc86e5034e2a0b2179e04ec7")]
        [TestCase(2048, "0100000001", "9fe416e838b5b72477c45b3db9cdccb37206137b966789ed6a5642ab", "ca0dd5e59b92437477f4471cdb0a111f1b4124e4a7f1ed2529fa860039efd0d362daf296586d93c39b8fe1c833462001012a583c34b58e363d88540f85d4ff849bf09adf0a9c8297da203510368f10e30309f74daa4f89038cfd5570559741b77d2652f01f2d562db4ecb676aaae32058443424520edea642ce1b7e7f685e429", "e4096df6c6c5681cd5e0b685a4ce1db1be619c2289df4c0aa5ba6cef1ceda002553af211d58b643a6cdafc5d909e42ca0086972bcd62b64764f8bb9d7ed19f39ce7066c9ac4d4885e77a4ff196c92dd2d91964d006768381792bb275aa8d5f49d7cf018807509076bc176e6ba54e1d65f9177c1a74a6cd1121b2a580df3aca5f")]
        [TestCase(2048, "0100000001", "aee72a62f306339f78ae5b319ed46fddfc00c67d735991194a546715", "ee91b851aed2744f3a38ec0b3e8b6f2bb963ca6c60ed14a0317c7b97499e8bd0e9b5f65e6306c7c720860b01a298c3a943db04afeb904f4074f4cff6c58434ab37633fb840da202d55dc0622e13f645c051f5083d5f375975c53abd72c2fe0c1f0276c36a41db9e68715bf999689c2e00ff24ab4a9ab4a43690e98574df0d061", "cece2b1c738774c000e5914dd31378f3cb73c02ada8cafb0d1d8ed5eb5477c28009fd4f65e2c69dd8eaab12ce6c923ae96a07510343d933c6b50200c76b3e029523abf6307126c6ef3987218d739f125e598a6d6fd735f3b302c61f7b85aa2b30ff0e7856dec051a1dcf5a20a09a9694e390db1be808850f0d8c31bdd8837105")]
        public void ShouldPassWithGoodParameters(int nlen, string e, string seed, string p, string q)
        {
            var subject = new RandomProvablePrimeGenerator(new HashFunction {Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160});
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(new BitString(p).ToPositiveBigInteger(), result.P, "p");
            Assert.AreEqual(new BitString(q).ToPositiveBigInteger(), result.Q, "q");
        }
    }
}
